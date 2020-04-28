using Microsoft.EntityFrameworkCore;
// using MySQL.Data.EntityFrameworkCore.Extensions;

namespace MetaWeather.Context
{
  public class LocationContext : DbContext
  {
    public LocationContext() {}

    public LocationContext(DbContextOptions<LocationContext> options)
        : base(options)
    {
  
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<LocationSearchResult>(entity =>
      {
        entity.HasKey(e => e.WOEID);
      });
    }
    public virtual DbSet<LocationSearchResult> locations { get; set; }
 }
}