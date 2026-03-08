using RecommendationApi.Models;

namespace RecommendationApi.Services;

/// <summary>
/// Contract for the recommendation service.
/// </summary>
public interface IRecommendationService
{
    /// <summary>
    /// Returns albums closest to the given preference scores, sorted by relevance ascending.
    /// </summary>
    List<AlbumDto> GetRecommendations(RecommendationRequest request, int count = 20);
}
