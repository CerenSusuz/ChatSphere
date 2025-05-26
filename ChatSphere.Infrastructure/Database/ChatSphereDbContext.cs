using Microsoft.EntityFrameworkCore;
using ChatSphere.Domain.Entities;

namespace ChatSphere.Infrastructure.Database;

public class ChatSphereDbContext(DbContextOptions<ChatSphereDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<ChatUserRoom> ChatUserRooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<ChatUserRoom>()
            .HasKey(x => new { x.UserId, x.RoomId });

        modelBuilder.Entity<ChatUserRoom>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Rooms)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<ChatUserRoom>()
            .HasOne(ur => ur.Room)
            .WithMany(r => r.Users)
            .HasForeignKey(ur => ur.RoomId);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.Room)
            .WithMany(r => r.Messages)
            .HasForeignKey(m => m.RoomId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<ChatMessage>().HasKey(m => m.Id);
        base.OnModelCreating(modelBuilder);
    }
}