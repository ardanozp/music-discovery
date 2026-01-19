namespace RecommendationApi.Models;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Year { get; set; }
    public string CoverUrl { get; set; } = string.Empty;
    public string WikipediaUrl { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
}
