using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace TaxCalculator.Repositories.DBContext;

public class UserDbContext : IdentityDbContext<IdentityUser>
{


    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUser>();
        base.OnModelCreating(builder);
    }

}
