using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts emotional positivity signal from valence audio feature.
/// Valence represents musical positiveness (happiness, cheerfulness).
/// Low valence = sad/dark, High valence = happy/bright.
/// </summary>
public class ValenceDepthExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        // If valence is not available, return neutral
        if (album.Valence == null)
        {
            return 0.5;
        }

        // Valence is already 0.0-1.0, just clamp to ensure valid range
        return Math.Clamp(album.Valence.Value, 0.0, 1.0);
    }
}
