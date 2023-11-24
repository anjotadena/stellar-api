using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class StellarDbContext : DbContext
{
    public StellarDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}