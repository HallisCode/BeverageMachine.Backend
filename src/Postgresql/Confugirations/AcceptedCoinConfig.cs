using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Postgresql.Converters;


namespace Postgresql.Confugirations
{
	public class AcceptedCoinConfig : IEntityTypeConfiguration<AcceptedCoin>
	{
		public void Configure(EntityTypeBuilder<AcceptedCoin> builder)
		{
			// Keys
			builder.HasKey(coin => coin.ID);

			builder.Property(coin => coin.ID).UseIdentityAlwaysColumn();

			// Properties
			builder.Property(coin => coin.RubleCoin).HasConversion<RubleCoinToInt>();
		}
	}
}
