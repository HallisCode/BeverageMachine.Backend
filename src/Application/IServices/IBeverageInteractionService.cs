using Application.DTO.BeverageInteraction;
using System.Threading.Tasks;

namespace Application.IServices
{
	/// <summary>
	/// Представляет сервис по принятию заказов на напитки.
	/// </summary>
	public interface IBeverageInteractionService
	{
		Task<ChangeResponse> AcceptOrderAsync(DrinkOrderRequest request);

		Task<AllDrinksResponse> GetAllDrinksAsync();

		Task<AllBlockedCoinsResponse> GetBlockedCoinsAsync();
	}
}
