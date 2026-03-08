using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RecommendationApi.Models;
using RecommendationApi.Services;

namespace RecommendationApi.Controllers;

/// <summary>
/// Controller for album recommendation endpoints.
/// Validates HTTP input, delegates all logic to the service layer.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    /// <summary>
    /// Returns albums ranked by closeness to the given preference scores.
    /// </summary>
    /// <param name="request">Preference scores (0.0–1.0) for energy, emotion, familiarity, time.</param>
    /// <param name="count">Number of results to return (default: 20, max: 100).</param>
    [HttpPost]
    [EnableRateLimiting("SessionsWritePolicy")]
    [RequestSizeLimit(8 * 1024)]
    public IActionResult GetRecommendations(
        [FromBody] RecommendationRequest request,
        [FromQuery] int count = 20)
    {
        if (request == null)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request body is required.",
                Type = "https://httpstatuses.com/400"
            });
        }

        if (!ModelState.IsValid ||
            float.IsNaN(request.Energy) || float.IsInfinity(request.Energy) ||
            float.IsNaN(request.Familiarity) || float.IsInfinity(request.Familiarity) ||
            float.IsNaN(request.Time) || float.IsInfinity(request.Time))
        {
            return ValidationProblem(ModelState);
        }

        count = Math.Clamp(count, 1, 100);

        var albums = _recommendationService.GetRecommendations(request, count);

        return Ok(new RecommendationResponse { Albums = albums });
    }
}
