using Domain.ValueObjects;
using REST.DTO;

namespace REST.Mapping
{
	public static class RubleCoinDtoMapping
	{
		public static RubleCoin MapToApplicationDTO(this RubleCoinDTO rubleCoinDTO)
		{
			return new RubleCoin(rubleCoinDTO.Value);
		}
	}
}
