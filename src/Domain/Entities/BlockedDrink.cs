

namespace Domain.Entities
{
	/// <summary>
	/// Описывает заблокированный напиток.
	/// </summary>
	public class BlockedDrink
	{
		// Relatinships
		public long DrinkID { get; private set; }


		// Navigations links
		public Drink Drink { get; private set; }

		// Logic
		private BlockedDrink() { }

		public BlockedDrink(long drinkId)
		{
			DrinkID = drinkId;
		}
	}
}
