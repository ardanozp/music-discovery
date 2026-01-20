using Microsoft.AspNetCore.Mvc;
using RecommendationApi.Models;
using RecommendationApi.Data;

namespace RecommendationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{

    [HttpPost]
    public IActionResult GetRecommendations([FromBody] RecommendationRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request is required.");
        }

        // STEP 1: Hard filter - Energy and Emotion must match (eliminates albums)
        var hardFiltered = AlbumData.Albums
            .Where(a => a.Energy == request.Energy && a.Emotion == request.Emotion)
            .ToList();

        // STEP 2: Soft scoring - Familiarity and Time add points (never eliminate)
        var scoredAlbums = hardFiltered.Select(album => new
        {
            Album = album,
            Score = CalculateSoftScore(album, request)
        }).ToList();

        // STEP 3: Sort by score DESC, then randomize within same score groups
        var recommendedAlbums = scoredAlbums
            .OrderByDescending(x => x.Score)
            .ThenBy(x => Guid.NewGuid()) // Random tie-breaking for equal scores
            .Select(x => x.Album)
            .ToList();

        return Ok(new { albums = recommendedAlbums });
    }

    private int CalculateSoftScore(Album album, RecommendationRequest request)
    {
        int score = 0;

        // Familiarity match: +2 points
        if (request.Familiarity.HasValue && album.Familiarity == request.Familiarity.Value)
        {
            score += 2;
        }

        // Time match: +1 point
        if (request.Time.HasValue && album.Time == request.Time.Value)
        {
            score += 1;
        }

        return score;
    }
}
