namespace RecommendationApi.Models;

/// <summary>
/// Response returned from POST /api/sessions and POST /api/sessions/{id}/restart.
/// </summary>
public class SessionResponse
{
    public Guid SessionId { get; set; }
    public List<AlbumDto> Albums { get; set; } = new();

    /// <summary>
    /// How many more times the user can call /restart.
    /// 0 means the next /restart will return 409 Conflict.
    /// </summary>
    public int RestartsRemaining { get; set; }
}
