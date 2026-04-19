using AuthPlayground.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthPlayground.Persistence.Contexts;

public sealed class AuthPlaygroundDbContext(DbContextOptions<AuthPlaygroundDbContext> options)
    : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthPlaygroundDbContext).Assembly);
    }
}
