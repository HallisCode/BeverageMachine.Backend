using Domain.ValueObjects;


namespace Application.DTO.BeverageInteraction
{
	public record DrinkOrderRequest(long drinkID, RubleCoin[] Coins);
}
