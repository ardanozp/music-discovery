using RecommendationApi.Models;
using RecommendationApi.Mappers;

namespace RecommendationApi.Services;

/// <summary>
/// Finds albums closest to user preferences using weighted distance scoring,
/// enforces artist diversity in the candidate pool, and adds controlled
/// stochastic jitter so the same query yields varied results across sessions.
/// </summary>
public class RecommendationService : IRecommendationService
{
    /// How many times larger the candidate pool is versus the final result count.
    /// e.g. count=3 → score top 15, then pick 3 randomly from those.
    private const int PoolFactor = 5;

    /// Maximum albums from a single artist allowed in the candidate pool.
    /// Prevents one popular artist from filling the results list.
    private const int MaxAlbumsPerArtist = 3;

    /// Fraction of each album's distance that is replaced by random noise.
    /// 0.12 = 12% jitter → same query gives noticeably different results each session
    /// while still staying close to the user's actual preferences.
    private const double JitterFactor = 0.12;

    /// Minimum year gap required between selected albums to ensure variety.
    private const int MinYearGap = 1;

    private readonly AlbumDataService _dataService;

    public RecommendationService(AlbumDataService dataService)
    {
        _dataService = dataService;
    }

    /// <inheritdoc/>
    public List<AlbumDto> GetRecommendations(RecommendationRequest request, int count = 20)
        => GetRecommendations(request, count, excludeMbids: null);

    /// <summary>
    /// Same as <see cref="GetRecommendations(RecommendationRequest, int)"/> but
    /// filters out albums whose MBID is in <paramref name="excludeMbids"/>.
    /// Used by session restarts to guarantee no repeats across rounds.
    /// </summary>
    public List<AlbumDto> GetRecommendations(
        RecommendationRequest request,
        int count,
        IReadOnlySet<string>? excludeMbids)
    {
        var poolSize = count * PoolFactor;

        // Hard Boundaries for Time:
        // "Past" sends a score ~0.30. "Now" sends a score ~0.80. "Timeless" sends ~0.50.
        // We enforce the 2010 boundary strictly here.
        bool isPast = request.Time <= 0.42f;
        bool isNow  = request.Time >= 0.60f;

        // Step 1: Compute a stochastic effective distance for every album.
        var scored = _dataService.ScoredAlbums
            .Where(a => excludeMbids == null || !excludeMbids.Contains(a.AlbumMbid))
            .Where(a => 
            {
                if (isPast && a.Year >= 2010) return false;
                if (isNow && a.Year < 2010) return false;
                return true;
            })
            .Select(a =>
            {
                var baseDistance = ScoringService.WeightedDistance(
                    a.EnergyScore, a.FamiliarityScore, a.TimeScore,
                    request.Energy, request.Familiarity, request.Time);

                var jitter        = Random.Shared.NextDouble() * JitterFactor;
                var effectiveDist = (1.0 - JitterFactor) * baseDistance + jitter;
                return (Album: a, Distance: effectiveDist);
            })
            .OrderBy(x => x.Distance);

        // Step 2 & 3: Selection
        var finalSelection = new List<ScoredAlbum>(count);
        var usedArtists = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Special Rule: If exactly 3 albums are requested and user chose "Past",
        // force one album from each specific era: <1985, 1985-1999, 2000-2009.
        if (isPast && count == 3)
        {
            ScoredAlbum? PickFromBucket(Func<ScoredAlbum, bool> predicate)
            {
                // To avoid always picking boundary years (e.g., exactly 1985 or 2000), 
                // we sort candidates in this bucket purely by Energy and Familiarity, 
                // completely ignoring the Time distance inside the bucket.
                var candidates = _dataService.ScoredAlbums
                    .Where(a => excludeMbids == null || !excludeMbids.Contains(a.AlbumMbid))
                    .Where(a => predicate(a) && !usedArtists.Contains(a.ArtistName ?? string.Empty))
                    .Select(a =>
                    {
                        var dE = a.EnergyScore - request.Energy;
                        var dF = a.FamiliarityScore - request.Familiarity;
                        var dist = Math.Sqrt(
                            ScoringService.WeightEnergy * dE * dE +
                            ScoringService.WeightFamiliarity * dF * dF);
                        var jitter = Random.Shared.NextDouble() * JitterFactor;
                        return (Album: a, Distance: (1 - JitterFactor) * dist + jitter);
                    })
                    .OrderBy(x => x.Distance)
                    .Take(PoolFactor * 2) // Take top 20 best-matching by Energy/Familiarity
                    .ToList();

                if (candidates.Count == 0) return null;
                
                var picked = candidates[Random.Shared.Next(candidates.Count)].Album;
                usedArtists.Add(picked.ArtistName ?? string.Empty);
                return picked;
            }

            var pre1985 = PickFromBucket(a => a.Year < 1985);
            var midEra  = PickFromBucket(a => a.Year >= 1985 && a.Year < 2000);
            var lateEra = PickFromBucket(a => a.Year >= 2000 && a.Year < 2010);

            if (pre1985 != null) finalSelection.Add(pre1985);
            if (midEra != null)  finalSelection.Add(midEra);
            if (lateEra != null) finalSelection.Add(lateEra);
        }

        // Fill remaining slots using generic logic (used for "Now", "Timeless", or if buckets failed)
        if (finalSelection.Count < count)
        {
            var pool = new List<ScoredAlbum>(poolSize);
            
            foreach (var (album, _) in scored)
            {
                if (pool.Count >= poolSize) break;
                
                var artistKey = album.ArtistName ?? string.Empty;
                if (usedArtists.Contains(artistKey)) continue;

                usedArtists.Add(artistKey);
                pool.Add(album);
            }

            foreach (var candidate in pool.OrderBy(_ => Random.Shared.Next()))
            {
                if (finalSelection.Count >= count) break;
                
                if (finalSelection.Any(selected => Math.Abs(selected.Year - candidate.Year) < MinYearGap))
                    continue;

                finalSelection.Add(candidate);
            }

            // Fallback: If strict MinYearGap filtering didn't yield enough, fill ignoring gap
            if (finalSelection.Count < count)
            {
                var needed = count - finalSelection.Count;
                var fill = pool.Except(finalSelection).Take(needed);
                finalSelection.AddRange(fill);
            }
        }

        // Optional: Shuffle so the order presented on screen isn't always oldest to newest
        finalSelection = finalSelection.OrderBy(_ => Random.Shared.Next()).ToList();

        return finalSelection.Select(AlbumMapper.ToAlbumDto).ToList();
    }
}
