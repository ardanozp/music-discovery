using DataPipeline.Data;
using DataPipeline.Enriched;
using DataPipeline.Pipeline;

namespace DataPipeline.Recommendation;

/// <summary>
/// Core engine for generating album recommendations.
/// Encapsulates the logic for selecting and ranking albums based on user preferences.
/// </summary>
public class RecommendationEngine
{
    private readonly AlbumEnrichmentPipeline _pipeline;

    public RecommendationEngine()
    {
        _pipeline = new AlbumEnrichmentPipeline();
    }

    /// <summary>
    /// Gets recommended albums sorted by relevance to the user's preferences.
    /// </summary>
    /// <param name="targetEnergy">User's preference for Energy (0.0 - 1.0)</param>
    /// <param name="targetEmotion">User's preference for Emotion (0.0 - 1.0)</param>
    /// <param name="targetFamiliarity">User's preference for Familiarity (0.0 - 1.0)</param>
    /// <param name="targetTime">User's preference for Time (0.0 - 1.0)</param>
    /// <returns>List of EnrichedAlbums sorted by distance (closest first)</returns>
    public List<EnrichedAlbum> GetRecommendations(float targetEnergy, float targetEmotion, float targetFamiliarity, float targetTime)
    {
        // 1. Load All Raw Data
        var rawAlbums = AlbumDataSource.Albums;

        // 2. Enrich All Albums
        var enrichedAlbums = new List<EnrichedAlbum>();
        foreach (var raw in rawAlbums)
        {
            enrichedAlbums.Add(_pipeline.Enrich(raw));
        }

        // 3. Calculate Distances and Rank
        // We use Manhattan Distance as the similarity metric
        var rankedAlbums = enrichedAlbums
            .Select(album => new
            {
                Album = album,
                Distance = CalculateManhattanDistance(album, targetEnergy, targetEmotion, targetFamiliarity, targetTime)
            })
            .OrderBy(x => x.Distance)
            .Select(x => x.Album)
            .ToList();

        return rankedAlbums;
    }

    private double CalculateManhattanDistance(EnrichedAlbum album, float targetEnergy, float targetEmotion, float targetFamiliarity, float targetTime)
    {
        double distance = 0.0;

        distance += Math.Abs(album.EnergyScore - targetEnergy);
        distance += Math.Abs(album.EmotionScore - targetEmotion);
        distance += Math.Abs(album.FamiliarityScore - targetFamiliarity);
        distance += Math.Abs(album.TimeScore - targetTime);

        return distance;
    }
}
