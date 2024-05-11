using Database.IRepositories;
using System.Threading.Tasks;

namespace Database.IUnitWork
{
	public interface IUnitWork
	{
		public IAcceptedCoinRepository AcceptedCoinRepository { get; }
		public IDrinkRepository DrinkRepository { get; }

		public IBlockedCoinRepository BlockedCoinRepository { get; }
		public IBlockedDrinkRepository BlockedDrinkRepository { get; }


		public Task<ITransaction> BeginTransactionAsync();
	}
}
