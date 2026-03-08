namespace RecommendationApi.Models;

/// <summary>
/// Holds the mutable state of a single user recommendation session.
/// Stored in-memory by SessionStore (singleton).
/// </summary>
public class SessionState
{
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>User preferences captured at session creation.</summary>
    public RecommendationRequest Preferences { get; init; } = new();

    /// <summary>
    /// How many restarts the user can still trigger.
    /// Starts at 2; decremented on each /restart call; errors at 0.
    /// </summary>
    public int RestartsRemaining { get; set; } = 1; 

    /// <summary>
    /// MBIDs of albums already shown to this user (across all rounds).
    /// Used to guarantee no repeats across initial + restart rounds.
    /// </summary>
    public HashSet<string> ShownMbids { get; } = new(StringComparer.OrdinalIgnoreCase);

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
