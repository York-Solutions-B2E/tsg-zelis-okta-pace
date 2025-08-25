using OktaCapstone.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace OktaCapstone.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Claim> Claims => Set<Claim>();
  public DbSet<Role> Roles => Set<Role>();
  public DbSet<SecurityEvent> SecurityEvents => Set<SecurityEvent>();
  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SecurityEvent>(b =>
    {
      b.HasOne(e => e.Author)
        .WithMany()
        .HasForeignKey(e => e.AuthorId)
        .OnDelete(DeleteBehavior.NoAction);

      b.HasOne(e => e.Target)
        .WithMany()
        .HasForeignKey(e => e.TargetId)
        .OnDelete(DeleteBehavior.NoAction);
    });
    base.OnModelCreating(modelBuilder);
  }
}
