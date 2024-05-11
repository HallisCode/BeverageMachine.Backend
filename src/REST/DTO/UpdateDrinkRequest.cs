using Microsoft.AspNetCore.Http;

namespace REST.DTO
{
	public record UpdateDrinkRequest(
		long DrinkId, string Title = null,
		IFormFile Image = null,
		int? Cost = null, int? Count = null
	);
}
