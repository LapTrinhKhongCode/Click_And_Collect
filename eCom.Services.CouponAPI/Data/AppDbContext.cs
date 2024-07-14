using eCom.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eCom.Services.CouponAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base() { }

		public DbSet<Coupon> Coupons { get; set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				CouponId = 1,	
				CouponCode = "10OFF",
				DiscountAmount = 10,
				MinAmount = 20
			});

			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				CouponId = 2,
				CouponCode = "20OFF",
				DiscountAmount = 20,
				MinAmount = 40
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
