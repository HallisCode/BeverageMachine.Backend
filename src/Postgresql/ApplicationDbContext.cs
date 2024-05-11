using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Postgresql
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<AcceptedCoin> AcceptedCoins { get; set; }
		public DbSet<Drink> Drinks { get; set; }

		public DbSet<BlockedDrink> BlockedDrinks { get; set; }
		public DbSet<BlockedCoin> BlockedCoins { get; set; }


		// Logic
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}
	}
}
