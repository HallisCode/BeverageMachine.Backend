namespace Domain.Entities
{
	public interface IEntityIdentifier<TKeyType>
	{
		public TKeyType ID { get; }
	}
}
