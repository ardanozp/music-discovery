using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using DataPipeline;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
        config.AddJsonFile("appsettings.json", optional: false))
    .ConfigureServices((ctx, services) =>
    {
        services.AddHttpClient();
        services.AddSingleton<LastFmClient>(sp =>
        {
            var apiKey = ctx.Configuration["LastFm:ApiKey"] ?? "";
            var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
            return new LastFmClient(http, apiKey);
        });
    })
    .Build();

var lastFm = host.Services.GetRequiredService<LastFmClient>();

// ─── Validate config ──────────────────────────────────────────────────────
var apiKey = host.Services.GetRequiredService<IConfiguration>()["LastFm:ApiKey"];
if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_LASTFM_API_KEY_HERE")
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: LastFM API key not set in appsettings.json");
    Console.ResetColor();
    return;
}

Console.WriteLine("=== Music Discovery: Artist Pipeline (LastFM Geo) ===\n");

// ─── Config ───────────────────────────────────────────────────────────────
var targetCountries = new[] { "united states", "united kingdom" };
const int    GeoLimit    = 50;     // Max per page for geo.getTopArtists
const int    MaxPerCountry = 2500; // Artists per country (US + UK = 5000 total)
const string ArtistsFile = "artists.json";

// ─── Load existing ────────────────────────────────────────────────────────
var artists = new List<SavedArtist>();
var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

if (File.Exists(ArtistsFile))
{
    try
    {
        var existing = JsonSerializer.Deserialize<List<SavedArtist>>(
            await File.ReadAllTextAsync(ArtistsFile),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (existing?.Count > 0)
        {
            artists = existing;
            foreach (var a in artists) seenNames.Add(a.Name);
            Console.WriteLine($"Loaded {artists.Count} existing artists.\n");
        }
    }
    catch { Console.WriteLine("Starting fresh.\n"); }
}

// ─── PHASE 1: Collect from geo.getTopArtists ─────────────────────────────
bool phase1Needed = artists.Count < (MaxPerCountry * targetCountries.Length);
if (phase1Needed)
{
    Console.WriteLine("── PHASE 1: Collecting artists by country (geo.getTopArtists) ──\n");

    foreach (var country in targetCountries)
    {
        // How many from this country are already saved?
        int alreadyFromCountry = artists.Count(a => a.Country == country);
        if (alreadyFromCountry >= MaxPerCountry)
        {
            Console.WriteLine($"  {country}: already have {alreadyFromCountry}, skipping.\n");
            continue;
        }

        int startPage = (alreadyFromCountry / GeoLimit) + 1;
        Console.WriteLine($"  [{country.ToUpper()}] Starting from page {startPage}");

        int page = startPage;
        int addedForCountry = alreadyFromCountry;

        while (addedForCountry < MaxPerCountry)
        {
            Console.Write($"    page {page}... ");

            var result = await lastFm.GetTopArtistsByCountryAsync(country, page, GeoLimit);
            var batch = result?.TopArtists?.Artist;

            if (batch == null || batch.Count == 0)
            {
                Console.WriteLine("empty — end of results.");
                break;
            }

            int added = 0;
            foreach (var artist in batch)
            {
                if (string.IsNullOrWhiteSpace(artist.Name)) continue;
                if (seenNames.Contains(artist.Name)) continue;

                seenNames.Add(artist.Name);
                artists.Add(new SavedArtist
                {
                    Name      = artist.Name,
                    Mbid      = string.IsNullOrWhiteSpace(artist.Mbid) ? null : artist.Mbid,
                    Listeners = artist.ListenerCount,
                    Country   = country,
                    Tags      = new List<string>()
                });
                added++;
                addedForCountry++;

                if (addedForCountry >= MaxPerCountry) break;
            }

            var totalPages = result?.TopArtists?.Attr?.TotalPages;
            Console.WriteLine($"{added} added ({addedForCountry}/{MaxPerCountry}) [pg {page}/{totalPages}]");

            await SaveAsync(ArtistsFile, artists);
            page++;
        }

        Console.WriteLine($"  [{country.ToUpper()}] Done: {addedForCountry} artists\n");
    }
}

// ─── PHASE 2: Enrich with tags (artist.getInfo) ───────────────────────────
var noTags = artists.Where(a => a.Tags.Count == 0).ToList();
if (noTags.Count > 0)
{
    Console.WriteLine($"── PHASE 2: Enriching {noTags.Count} artists with tags ──\n");
    int done = 0;

    foreach (var artist in noTags)
    {
        Console.Write($"  [{done + 1}/{noTags.Count}] {artist.Name}... ");

        var info = await lastFm.GetArtistInfoAsync(artist.Name);
        if (info != null)
        {
            artist.Tags = info.Tags?.Tag?
                .Where(t => !string.IsNullOrWhiteSpace(t.Name))
                .Select(t => t.Name!.ToLowerInvariant())
                .Take(5)
                .ToList() ?? new List<string>();

            if (info.Stats?.ListenerCount > 0)
                artist.Listeners = info.Stats.ListenerCount;

            Console.WriteLine($"tags: [{string.Join(", ", artist.Tags.Take(3))}]");
        }
        else
        {
            Console.WriteLine("not found");
        }

        done++;

        // Save every 50 artists
        if (done % 50 == 0)
            await SaveAsync(ArtistsFile, artists);
    }

    await SaveAsync(ArtistsFile, artists);
}

Console.WriteLine($"\n✓ Done. {artists.Count} artists in {ArtistsFile}");
Console.WriteLine($"  US:  {artists.Count(a => a.Country == "united states")}");
Console.WriteLine($"  UK:  {artists.Count(a => a.Country == "united kingdom")}");
Console.WriteLine($"  With tags: {artists.Count(a => a.Tags.Count > 0)}");

// ─── Helpers ──────────────────────────────────────────────────────────────
static async Task SaveAsync<T>(string path, List<T> data)
{
    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(path, json);
}
