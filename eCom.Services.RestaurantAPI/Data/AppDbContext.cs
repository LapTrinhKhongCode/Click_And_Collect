using eCom.Services.RestaurantAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace eCom.Services.RestaurantAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base() { }

        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Restaurant>().HasData(new Restaurant
            {
                RestaurantId = 1,
                RestaurantName = "Ramen Ichido",
                RestaurantRating = 4.3,
                RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                RestaurantLocation = "Da Nang"
            });

            modelBuilder.Entity<Restaurant>().HasData(new Restaurant
            {
                RestaurantId = 2,
                RestaurantName = "Udon Yukichi",
                RestaurantRating = 4.6,
                RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                RestaurantLocation = "Tokyo"
            });

            modelBuilder.Entity<Restaurant>().HasData(new Restaurant
            {
                RestaurantId = 3,
                RestaurantName = "Gyudon",
                RestaurantRating = 4.7,
                RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                RestaurantLocation = "Fukuoka"
            });

            modelBuilder.Entity<Restaurant>().HasData(new Restaurant
            {
                RestaurantId = 4,
                RestaurantName = "Tenpura",
                RestaurantRating = 4.9,
                RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                RestaurantLocation = "Ishikawa"
            });
        }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				IConfigurationRoot config = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json")
				   .Build();
				string connString = config.GetConnectionString("DefaultConnection");
				optionsBuilder.UseSqlServer(connString); 
			}
		}
	}
}
