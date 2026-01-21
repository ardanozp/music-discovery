using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts harmonic complexity/depth signal from audio features.
/// Uses acousticness and energy to estimate harmonic richness.
/// Higher acousticness + moderate energy → higher harmonic depth.
/// Very high or very low energy → lower harmonic depth.
/// </summary>
public class HarmonicDepthExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        // If no audio features available, return neutral
        if (album.Acousticness == null && album.Energy == null)
        {
            return 0.5;
        }

        var acousticness = album.Acousticness ?? 0.5;
        var energy = album.Energy ?? 0.5;

        // Harmonic depth is higher when:
        // 1. Acousticness is high (acoustic instruments have rich harmonics)
        // 2. Energy is moderate (extreme energy often reduces harmonic complexity)
        
        // Calculate energy penalty (extreme values reduce depth)
        // Optimal energy for harmonic depth is around 0.4-0.6
        var energyDistance = Math.Abs(energy - 0.5);
        var energyFactor = 1.0 - (energyDistance * 0.8); // Penalty for extreme energy

        // Combine: 70% acousticness, 30% energy factor
        var depth = (acousticness * 0.7) + (energyFactor * 0.3);

        return Math.Clamp(depth, 0.0, 1.0);
    }
}
