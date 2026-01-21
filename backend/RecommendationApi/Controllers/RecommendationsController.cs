using Microsoft.AspNetCore.Mvc;
using RecommendationApi.Models;
using RecommendationApi.Services;

namespace RecommendationApi.Controllers;

/// <summary>
/// Controller for album recommendation endpoints.
/// 
/// RESPONSIBILITIES:
/// - Validate HTTP request
/// - Call service layer
/// - Map to HTTP response
/// 
/// CONSTRAINTS:
/// - NO business logic
/// - NO direct pipeline access
/// - NO scoring or ranking
/// 
/// This is a thin controller that delegates all work to the service layer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;
    
    /// <summary>
    /// Initializes the controller with the recommendation service.
    /// </summary>
    public RecommendationsController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    /// <summary>
    /// Gets album recommendations based on user preferences.
    /// </summary>
    /// <param name="request">User preference scores (0.0 - 1.0 for each dimension)</param>
    /// <returns>List of recommended albums sorted by relevance</returns>
    [HttpPost]
    public IActionResult GetRecommendations([FromBody] RecommendationRequest request)
    {
        // Validate request
        if (request == null)
        {
            return BadRequest("Request is required.");
        }
        
        // Validate score ranges (0.0 - 1.0)
        if (request.Energy < 0 || request.Energy > 1 ||
            request.Emotion < 0 || request.Emotion > 1 ||
            request.Familiarity < 0 || request.Familiarity > 1 ||
            request.Time < 0 || request.Time > 1)
        {
            return BadRequest("All preference scores must be between 0.0 and 1.0.");
        }
        
        // Call service to get recommendations
        var albums = _recommendationService.GetRecommendations(request);
        
        // Build response
        var response = new RecommendationResponse
        {
            Albums = albums
        };
        
        return Ok(response);
    }
}
