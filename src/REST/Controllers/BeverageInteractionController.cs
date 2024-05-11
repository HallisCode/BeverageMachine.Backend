using Application.DTO.BeverageInteraction;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using REST.Mapping;
using System.Threading.Tasks;

namespace REST.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class BeverageInteractionController : ControllerBase
	{
		private readonly IBeverageInteractionService _beverageInteractionService;


		public BeverageInteractionController(IBeverageInteractionService beverageInteractionService)
		{
			this._beverageInteractionService = beverageInteractionService;
		}

		[HttpGet]
		public async Task<ActionResult<AllDrinksResponse>> GetAllDrinks()
		{
			return await _beverageInteractionService.GetAllDrinksAsync();
		}

		[HttpGet]
		public async Task<ActionResult<AllBlockedCoinsResponse>> GetAllBlockedCoins()
		{
			return await _beverageInteractionService.GetBlockedCoinsAsync();
		}

		[HttpPost]
		public async Task<ActionResult<ChangeResponse>> TakeOrder([FromBody] DTO.DrinkOrderRequest request)
		{
			return await _beverageInteractionService.AcceptOrderAsync(request.MapToApplicationDTO());
		}
	}
}
