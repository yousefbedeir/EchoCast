namespace EchoCast.Domain.Entities;

public class Rating
{
    public int Id { get; set; }
    public int Stars { get; set; }

    public int PodcastId { get; set; }
    public Podcast Podcast { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

