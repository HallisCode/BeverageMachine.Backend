namespace Application.DTO
{
	public record DrinkDTO(long ID, string Title, string ImageName, int Cost, int Count, bool isBlocked);
}
