using Microsoft.EntityFrameworkCore;
using ChatSphere.Domain.Entities;

namespace ChatSphere.Infrastructure.Database;

public class ChatSphereDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }

    public DbSet<Room> Rooms { get; set; }


    public ChatSphereDbContext(DbContextOptions<ChatSphereDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<ChatMessage>().HasKey(m => m.Id);
        base.OnModelCreating(modelBuilder);
    }
}