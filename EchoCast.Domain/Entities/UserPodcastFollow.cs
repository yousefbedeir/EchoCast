namespace EchoCast.Domain.Entities;

public class UserPodcastFollow
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int PodcastId { get; set; }
    public Podcast Podcast { get; set; } = null!;
}

