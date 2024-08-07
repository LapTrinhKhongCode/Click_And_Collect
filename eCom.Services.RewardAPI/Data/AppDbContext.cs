using eCom.Services.RewardAPI.Models;


using Microsoft.EntityFrameworkCore;

namespace eCom.Services.RewardAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base() { }

		public DbSet<Rewards> Rewards { get; set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
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
