namespace EchoCast.Domain.Entities;

public class Podcast
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<UserPodcastFollow> Followers { get; set; } = new List<UserPodcastFollow>();
}

