namespace REST.DTO
{
	public record ExceptionDTO(string TypeError, string Title, object Details = null);
}
