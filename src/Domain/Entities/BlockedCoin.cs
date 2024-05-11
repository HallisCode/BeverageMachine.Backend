using Domain.ValueObjects;


namespace Domain.Entities
{
	// Описывает заблокированную монету.
	public class BlockedCoin
	{
		public RubleCoin RubleCoin { get; private set; }


		// Logic
		private BlockedCoin() { }

		public BlockedCoin(RubleCoin rubleCoin)
		{
			RubleCoin = rubleCoin;
		}
	}
}
