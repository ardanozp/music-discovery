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

        // Step 1: Compute a stochastic effective distance for every album.
        //   effectiveDistance = (1 - JitterFactor) * weightedDistance + JitterFactor * noise
        //   This nudges the ordering slightly each call, producing session variety.
        var scored = _dataService.ScoredAlbums
            .Where(a => excludeMbids == null || !excludeMbids.Contains(a.AlbumMbid))
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

        // Step 2: Build the candidate pool with artist diversity enforcement.
        //   Walk the sorted list; accept at most MaxAlbumsPerArtist per artist.
        var artistCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var pool         = new List<ScoredAlbum>(poolSize);

        foreach (var (album, _) in scored)
        {
            if (pool.Count >= poolSize) break;

            var artistKey = album.ArtistName ?? string.Empty;
            artistCounts.TryGetValue(artistKey, out var existing);
            if (existing >= MaxAlbumsPerArtist) continue;

            artistCounts[artistKey] = existing + 1;
            pool.Add(album);
        }

        // Step 3: Randomly sample `count` from the diverse pool.
        return pool
            .OrderBy(_ => Random.Shared.Next())
            .Take(count)
            .Select(AlbumMapper.ToAlbumDto)
            .ToList();
    }
}
