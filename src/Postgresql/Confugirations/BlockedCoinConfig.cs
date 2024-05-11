using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Postgresql.Converters;


namespace Postgresql.Confugirations
{
	internal class BlockedCoinConfig : IEntityTypeConfiguration<BlockedCoin>
	{
		public void Configure(EntityTypeBuilder<BlockedCoin> builder)
		{
			// Keys
			builder.HasKey(coin => coin.RubleCoin);

			// Properties
			builder.Property(coin => coin.RubleCoin).HasConversion<RubleCoinToInt>();
		}
	}
}
