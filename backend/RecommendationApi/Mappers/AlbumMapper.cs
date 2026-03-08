using RecommendationApi.Models;

namespace RecommendationApi.Mappers;

/// <summary>
/// Maps between ScoredAlbum (internal) and AlbumDto (API response).
/// Pure, logic-free transporter.
/// </summary>
public static class AlbumMapper
{
    public static AlbumDto ToAlbumDto(ScoredAlbum album) => new()
    {
        Id               = album.AlbumMbid ?? string.Empty,
        Title            = album.Title ?? string.Empty,
        Artist           = album.ArtistName ?? string.Empty,
        Year             = album.Year,
        CoverUrl         = album.CoverUrl ?? string.Empty,
        WikipediaUrl     = GenerateWikipediaUrl(album.Title, album.ArtistName),
        EnergyScore      = album.EnergyScore,
        FamiliarityScore = album.FamiliarityScore,
        TimeScore        = album.TimeScore
    };

    private static string GenerateWikipediaUrl(string? title, string? artist)
    {
        // Point to our internal redirect endpoint which queries the Wikipedia API.
        // Needs URL encoding to safely pass album and artist names via querystring.
        var encodedTitle = Uri.EscapeDataString(title ?? string.Empty);
        var encodedArtist = Uri.EscapeDataString(artist ?? string.Empty);
        
        return $"/api/wikipedia/redirect?album={encodedTitle}&artist={encodedArtist}";
    }
}
