using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPIProject.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readeRoleId = "9636c525-3800-4a7a-90e0-a4c7cde8c105";
            var writerRoleId = "068b5986-3254-4616-94fc-b2a40de985ad";

            var roles = new List<IdentityRole>
            { 
                new IdentityRole
                {
                    Id = readeRoleId,
                    ConcurrencyStamp = readeRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                  new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
