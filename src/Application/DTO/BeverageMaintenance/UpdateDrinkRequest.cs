namespace Application.DTO.BeverageMaintenance
{
	public record UpdateDrinkRequest(long ID, string Title = null, ImageDTO Image = null, int? Cost = null, int? Count = 0);
}
