using Microsoft.AspNetCore.Http;

namespace REST.DTO
{
	public record AddDrinkRequest(string Title, IFormFile Image, int Cost, int Count = 0);

}
