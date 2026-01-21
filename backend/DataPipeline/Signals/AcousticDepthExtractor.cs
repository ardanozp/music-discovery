using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts acoustic vs. electronic signal from acousticness audio feature.
/// Higher acousticness = more organic, natural sound.
/// Lower acousticness = more electronic, synthetic sound.
/// </summary>
public class AcousticDepthExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        // If acousticness is not available, return neutral
        if (album.Acousticness == null)
        {
            return 0.5;
        }

        // Acousticness is already 0.0-1.0, just clamp to ensure valid range
        return Math.Clamp(album.Acousticness.Value, 0.0, 1.0);
    }
}
