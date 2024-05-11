using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Postgresql.Confugirations
{
	public class BlockedDrinkConfig : IEntityTypeConfiguration<BlockedDrink>
	{
		public void Configure(EntityTypeBuilder<BlockedDrink> builder)
		{
			// Keys
			builder.HasKey(drink => drink.DrinkID);

			// Relationships
			builder.HasOne<Drink>(drink => drink.Drink)
				.WithOne()
				.HasForeignKey<BlockedDrink>(drink => drink.DrinkID)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
