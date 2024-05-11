using Domain.ValueObjects;
using System.Linq;

namespace REST.Mapping
{
	public static class DrinkOrderRequestMapping
	{
		public static Application.DTO.BeverageInteraction.DrinkOrderRequest MapToApplicationDTO(this DTO.DrinkOrderRequest request)
		{
			RubleCoin[] coins = request.Coins.Select(coin => new RubleCoin(coin.Value)).ToArray();

			return new Application.DTO.BeverageInteraction.DrinkOrderRequest(request.DrinkId, coins);
		}

	}
}
