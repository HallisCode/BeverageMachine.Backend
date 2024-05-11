using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Postgresql.Converters
{
	public class RubleCoinToInt : ValueConverter<RubleCoin, int>
	{
		public RubleCoinToInt() : base(coin => coin.Value, _int => new RubleCoin(_int)) { }
	}
}
