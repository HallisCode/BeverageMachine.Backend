namespace Application.DTO.BeverageMaintenance
{
	// TODO : Зашить логику валидации, чтобы количетсво не могло быть отрицательным
	public record ChangeNumberCoinsRequest(
		int? NumberOneRuble = null, int? NumberTwoRuble = null,
		int? NumberFiveRuble = null, int? numberTenRuble = null
		);

}
