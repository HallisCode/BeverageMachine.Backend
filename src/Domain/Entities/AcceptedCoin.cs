using Domain.ValueObjects;


namespace Domain.Entities
{
	/// <summary>
	/// Описывает принятую монету.
	/// </summary>
	public class AcceptedCoin : IEntityIdentifier<long>
	{
		public long ID { get; private set; }

		public RubleCoin RubleCoin { get; private set; }

		// Logic

		private AcceptedCoin() { }

		public AcceptedCoin(RubleCoin rubleCoin)
		{
			RubleCoin = rubleCoin;
		}
	}
}
