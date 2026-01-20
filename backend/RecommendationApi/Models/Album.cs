namespace RecommendationApi.Models;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Year { get; set; }
    public string CoverUrl { get; set; } = string.Empty;
    public string WikipediaUrl { get; set; } = string.Empty;
    public EnergyLevel Energy { get; set; }
    public EmotionLevel Emotion { get; set; }
    public FamiliarityLevel Familiarity { get; set; }
    public TimeFeeling Time { get; set; }
}
