using FirstWebAPIProject.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPIProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        
        public DbSet<Difficulty> Difficulty { get; set; }

        public DbSet<Region> Region { get; set; }

        public DbSet<Walk> Walk { get; set; }

        public DbSet<Image> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data for Difficulty - Easy,Medium, Hard 

            var difficulties = new List<Difficulty>()
           {
               new Difficulty()
               {
                   Id = Guid.Parse("9584a02c-070a-414e-a362-aaede9cdb09e"),
                   Name = "Easy"
               },
                new Difficulty()
               {
                   Id = Guid.Parse("8a7c79fc-c65e-4a94-a1cf-76ca0267c45f"),
                   Name = "Medium"
               },
                 new Difficulty()
               {
                   Id = Guid.Parse("21e3b820-28bf-443b-8205-76cc2e0931f9"),
                   Name = "Hard"
               }

           };

            // Seed Difficulties to the Database

            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImgUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImgUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImgUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImgUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImgUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImgUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);

        }

    }
}
