namespace EchoCast.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public ICollection<Podcast> CreatedPodcasts { get; set; } = new List<Podcast>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<UserPodcastFollow> Follows { get; set; } = new List<UserPodcastFollow>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

