using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts familiarity signal from label type.
/// Major labels → higher familiarity (more mainstream).
/// Indie labels → lower familiarity (more niche).
/// Unknown → neutral.
/// </summary>
public class LabelScaleExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        // Parse string to internal enum, default to Unknown
        if (!Enum.TryParse<LabelType>(album.LabelType, true, out var labelType))
        {
            labelType = LabelType.Unknown;
        }

        return labelType switch
        {
            LabelType.Major => 0.8,    // Major labels are mainstream
            LabelType.Indie => 0.3,    // Indie labels are niche
            LabelType.Unknown => 0.5,  // Unknown = neutral
            _ => 0.5                   // Default to neutral
        };
    }
}
