using AuthPlayground.Domain.Entities;
using AuthPlayground.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthPlayground.Persistence.Contexts;

public sealed class AuthPlaygroundDbContext(DbContextOptions<AuthPlaygroundDbContext> options)
    : IdentityDbContext<AppUser, AppRole, string>(options)
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthPlaygroundDbContext).Assembly);
    }
}
