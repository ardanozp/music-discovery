using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using RecommendationApi.Models;
using RecommendationApi.Services;

namespace RecommendationApi.Controllers;

/// <summary>
/// Manages user recommendation sessions.
/// Each session holds 3 albums and allows up to 2 restarts.
/// Anonymous users are limited to 1 session per day via X-Anonymous-Id header.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private const string AnonIdHeader = "X-Anonymous-Id";
    private static readonly Regex AnonIdPattern = new("^[a-zA-Z0-9_-]{16,128}$", RegexOptions.Compiled);

    private readonly SessionStore _store;
    private readonly AnonymousUserStore _anonStore;
    private readonly bool _bypassDailyLimit;

    public SessionsController(SessionStore store, AnonymousUserStore anonStore, IConfiguration configuration)
    {
        _store            = store;
        _anonStore        = anonStore;
        _bypassDailyLimit = configuration.GetValue<bool>("BypassDailyLimit");
    }

    /// <summary>
    /// Creates a new session with the given preferences.
    /// Requires X-Anonymous-Id header. Limited to 1 session per day per user.
    /// Returns 3 album recommendations, a session ID, and restart count.
    /// </summary>
    [HttpPost]
    [EnableRateLimiting("IpBasedSessionPolicy")]
    [RequestSizeLimit(8 * 1024)]
    public IActionResult CreateSession([FromBody] RecommendationRequest preferences)
    {
        if (preferences == null)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Request body is required.",
                Type = "https://httpstatuses.com/400"
            });
        }

        if (!ModelState.IsValid ||
            float.IsNaN(preferences.Energy) || float.IsInfinity(preferences.Energy) ||
            float.IsNaN(preferences.Familiarity) || float.IsInfinity(preferences.Familiarity) ||
            float.IsNaN(preferences.Time) || float.IsInfinity(preferences.Time))
        {
            return ValidationProblem(ModelState);
        }

        // Require an anonymous ID from the client
        if (!Request.Headers.TryGetValue(AnonIdHeader, out var anonIdValues) ||
            string.IsNullOrWhiteSpace(anonIdValues.FirstOrDefault()))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = $"Header '{AnonIdHeader}' is required.",
                Type = "https://httpstatuses.com/400"
            });
        }

        var anonymousId = anonIdValues.First()!.Trim();
        if (!AnonIdPattern.IsMatch(anonymousId))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid anonymous id format.",
                Type = "https://httpstatuses.com/400"
            });
        }

        // Enforce atomically to avoid race-condition bypass.
        // In development, BypassDailyLimit=true skips the quota so testing is unrestricted.
        if (!_bypassDailyLimit && !_anonStore.TryConsumeDailyQuota(anonymousId))
        {
            return StatusCode(429, new ProblemDetails
            {
                Status = StatusCodes.Status429TooManyRequests,
                Title = "Daily quota exceeded.",
                Detail = "Please try again later.",
                Type = "https://httpstatuses.com/429"
            });
        }

        var response = _store.CreateSession(preferences);
        return Ok(response);
    }

    /// <summary>
    /// Restarts a session: returns 3 new albums, none previously shown.
    /// Up to 2 restarts are allowed per session.
    /// </summary>
    /// <param name="id">Session ID returned from POST /api/sessions.</param>
    [HttpPost("{id}/restart")]
    public IActionResult Restart(Guid id)
    {
        try
        {
            var response = _store.Restart(id);

            if (response == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Session not found.",
                    Type = "https://httpstatuses.com/404"
                });
            }

            return Ok(response);
        }
        catch (InvalidOperationException)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Session cannot be restarted.",
                Detail = "Restart limit reached for this session.",
                Type = "https://httpstatuses.com/409"
            });
        }
    }
}
