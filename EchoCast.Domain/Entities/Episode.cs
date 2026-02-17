namespace EchoCast.Domain.Entities;

public class Episode
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public int PodcastId { get; set; }
    public Podcast Podcast { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}

