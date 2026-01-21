using DataPipeline.Enriched;
using RecommendationApi.Models;

namespace RecommendationApi.Services;

/// <summary>
/// Interface for the recommendation service.
/// Defines the contract for getting album recommendations based on user preferences.
/// </summary>
public interface IRecommendationService
{
    /// <summary>
    /// Gets album recommendations based on user preferences.
    /// </summary>
    /// <param name="request">User preference scores</param>
    /// <returns>List of recommended albums sorted by relevance</returns>
    List<AlbumDto> GetRecommendations(RecommendationRequest request);
}
