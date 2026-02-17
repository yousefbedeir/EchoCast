using EchoCast.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Podcast> Podcasts => Set<Podcast>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UserPodcastFollow> UserPodcastFollows => Set<UserPodcastFollow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.CreatedPodcasts)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Podcast>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.About)
                .HasMaxLength(2000);

            entity.HasMany(p => p.Episodes)
                .WithOne(e => e.Podcast)
                .HasForeignKey(e => e.PodcastId);

            entity.HasMany(p => p.Ratings)
                .WithOne(r => r.Podcast)
                .HasForeignKey(r => r.PodcastId);
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(4000);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Episode)
                .HasForeignKey(c => c.EpisodeId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Stars)
                .IsRequired();

            entity.HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);

            entity.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserPodcastFollow>(entity =>
        {
            entity.HasKey(f => new { f.UserId, f.PodcastId });

            entity.HasOne(f => f.User)
                .WithMany(u => u.Follows)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Podcast)
                .WithMany(p => p.Followers)
                .HasForeignKey(f => f.PodcastId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

