using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Postgresql.Confugirations
{
	public class DrinkConfig : IEntityTypeConfiguration<Drink>
	{
		public void Configure(EntityTypeBuilder<Drink> builder)
		{
			// Keys
			builder.HasKey(drink => drink.ID);

			builder.Property(drink => drink.ID).UseIdentityAlwaysColumn();

			// Indexes
			builder.HasIndex(drink => drink.Title).IsUnique();

			// Properties
			builder.Property(drink => drink.Title).IsRequired().HasMaxLength(32);

			builder.Property(drink => drink.ImageName).IsRequired().HasMaxLength(512);

			builder.Property(drink => drink.Count).IsRequired();

			builder.Property(drink => drink.Cost).IsRequired();
		}
	}
}
