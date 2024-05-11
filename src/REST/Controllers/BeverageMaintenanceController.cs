using Application.DTO;
using Application.DTO.BeverageMaintenance;
using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using REST.AccessRules;
using REST.DTO;
using REST.Mapping;
using System.Threading.Tasks;

namespace REST.Controllers
{
	[QueryKeyAccess]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class BeverageMaintenanceController : ControllerBase
	{
		private readonly IBeverageMaintetanceService _beverageMaintetanceService;


		public BeverageMaintenanceController(IBeverageMaintetanceService beverageMaintetanceService)
		{
			this._beverageMaintetanceService = beverageMaintetanceService;
		}

		[HttpGet]
		public async Task<ActionResult<NumberCoinsDTO>> GetNumberCoins()
		{
			return await _beverageMaintetanceService.GetNumberCoinsAsync();
		}

		[HttpPost]
		public async Task<ActionResult<NumberCoinsDTO>> ChangeNumberCoins([FromBody] ChangeNumberCoinsRequest request)
		{
			return await _beverageMaintetanceService.ChangeNumberCoinsAsync(request);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateDrink([FromForm] DTO.UpdateDrinkRequest request)
		{
			await _beverageMaintetanceService.UpdateDrinkAsync(await request.MapToApplicationDTO());

			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult<DrinkDTO>> AddDrink([FromForm] DTO.AddDrinkRequest request)
		{
			return await _beverageMaintetanceService.AddDrinkAsync(await request.MapToApplicationDTO());
		}

		[HttpPost]
		public async Task<ActionResult> DeleteDrink([FromBody] DeleteDrinkRequest request)
		{
			await _beverageMaintetanceService.DeleteDrinkAsync(request);

			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult> LockUnlockDrink([FromBody] LockUnlockDrinkRequest request)
		{
			await _beverageMaintetanceService.LockUnlockDrinkAsync(request);

			return Ok();
		}

		[HttpPost]
		public async Task<ActionResult> LockUnlockCoin([FromBody] RubleCoinDTO request)
		{
			await _beverageMaintetanceService.LockUnlockCoinAsync(new LockUnlockCoinRequest(request.MapToApplicationDTO()));

			return Ok();
		}
	}
}
