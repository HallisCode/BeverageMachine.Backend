using Application.DTO;
using Application.DTO.BeverageMaintenance;
using System.Threading.Tasks;


namespace Application.IServices
{
	public interface IBeverageMaintetanceService
	{
		Task<DrinkDTO> AddDrinkAsync(AddDrinkRequest request);

		Task<NumberCoinsDTO> ChangeNumberCoinsAsync(ChangeNumberCoinsRequest request);

		Task<NumberCoinsDTO> GetNumberCoinsAsync();

		Task DeleteDrinkAsync(DeleteDrinkRequest request);

		Task UpdateDrinkAsync(UpdateDrinkRequest request);

		Task LockUnlockDrinkAsync(LockUnlockDrinkRequest request);

		Task LockUnlockCoinAsync(LockUnlockCoinRequest request);
	}
}
