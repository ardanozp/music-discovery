using RecommendationApi.Models;
using RecommendationApi.Mappers;
using DataPipeline.Recommendation;

namespace RecommendationApi.Services;

/// <summary>
/// Service layer for album recommendations.
/// Bridges the HTTP layer and the DataPipeline RecommendationEngine.
/// </summary>
public class RecommendationService : IRecommendationService
{
    private readonly RecommendationEngine _engine;
    
    public RecommendationService()
    {
        // In a real app we might inject this, but for now we instantiate directly
        // because it's stateless and part of the referenced DataPipeline library.
        _engine = new RecommendationEngine();
    }
    
    public List<AlbumDto> GetRecommendations(RecommendationRequest request)
    {
        // Delegate to DataPipeline's engine
        var enrichedAlbums = _engine.GetRecommendations(
            request.Energy,
            request.Emotion,
            request.Familiarity,
            request.Time
        );
        
        // Map to API DTOs
        var albumDtos = enrichedAlbums
            .Select(AlbumMapper.ToAlbumDto)
            .ToList();
        
        return albumDtos;
    }
}
