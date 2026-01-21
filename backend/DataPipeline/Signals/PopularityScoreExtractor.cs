using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts familiarity signal from popularity score.
/// Normalizes popularity (0-100) to 0.0-1.0 scale.
/// Higher popularity = more familiar/mainstream.
/// </summary>
public class PopularityScoreExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        // Popularity is 0-100, normalize to 0.0-1.0
        // Clamp to ensure we stay in valid range
        var normalized = album.Popularity / 100.0;
        return Math.Clamp(normalized, 0.0, 1.0);
    }
}
