using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Bug> Bugs { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bug>()
            .HasOne(b => b.CreatedBy)
            .WithMany(u => u.Bugs)
            .HasForeignKey(b => b.UserId);
    }
}