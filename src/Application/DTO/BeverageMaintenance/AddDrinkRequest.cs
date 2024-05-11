namespace Application.DTO.BeverageMaintenance
{
	public record AddDrinkRequest(string Title, ImageDTO Image, int Cost, int Count = 0);
}
