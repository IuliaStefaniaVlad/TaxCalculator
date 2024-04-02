using Microsoft.EntityFrameworkCore;
using TaxCalculator.Repositories.Entities;

namespace TaxCalculator.Repositories.DBContext;


public class CountryTaxBandsDbContext : DbContext
{

    // Important: 
    // Update-Database -Project TaxCalculator.Repositories -StartupProject TaxCalculator.Api -Context CountryTaxBandsDbContext
    // This exotic command was required because the repository is not the entry point of the application and some more reasons that I cannot really understand why
    public CountryTaxBandsDbContext(DbContextOptions<CountryTaxBandsDbContext> options) : base(options) { }

    public DbSet<CountryTaxBandsEntity> CountryTaxBands{ get; set; }
    public DbSet<TaxBandEntity> TaxBands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Define PKs
        modelBuilder.Entity<CountryTaxBandsEntity>().HasKey(x => x.CountryTaxBandsId);
        modelBuilder.Entity<TaxBandEntity>().HasKey(x => x.TaxBandId);
        modelBuilder.Entity<CountryTaxBandsEntity>().HasIndex(x => x.Country)
                                                    .IsUnique(); //for this line i added .HasIndex() for the Country field to be unique
        //Define relationship
        modelBuilder.Entity<CountryTaxBandsEntity>()
                    .HasMany(e => e.TaxBands)
                    .WithOne(e => e.CountryTaxBand)
                    .HasForeignKey(e => e.CountryTaxBandId)
                    .IsRequired();
    }

}


