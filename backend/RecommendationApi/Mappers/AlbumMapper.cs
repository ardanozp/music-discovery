using RecommendationApi.Models;
using DataPipeline.Enriched;

namespace RecommendationApi.Mappers;

/// <summary>
/// Maps between EnrichedAlbum (Pipeline Output) and AlbumDto (API Response).
/// Pure transporter, logic-free.
/// </summary>
public static class AlbumMapper
{
    /// <summary>
    /// Maps an EnrichedAlbum (DataPipeline output) to AlbumDto (API response).
    /// </summary>
    public static AlbumDto ToAlbumDto(EnrichedAlbum enriched)
    {
        return new AlbumDto
        {
            Id = int.Parse(enriched.Id), // Assuming Id is int-parseable based on legacy data
            Title = enriched.Title,
            Artist = enriched.Artist,
            Year = enriched.Year,
            
            // Metadata now comes from pipeline
            CoverUrl = enriched.CoverUrl,
            WikipediaUrl = enriched.WikipediaUrl,
            
            // Scores (Floats)
            EnergyScore = (float)enriched.EnergyScore,
            EmotionScore = (float)enriched.EmotionScore,
            FamiliarityScore = (float)enriched.FamiliarityScore,
            TimeScore = (float)enriched.TimeScore
        };
    }
}
