namespace EchoCast.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;

    public int EpisodeId { get; set; }
    public Episode Episode { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

