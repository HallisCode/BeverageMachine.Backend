using Database.IQuery.Select;
using Domain.Entities;

namespace Database.IRepositories
{
	public interface IBlockedCoinRepository : IBaseOperations<BlockedCoin>, IQuerySelectAll<BlockedCoin>
	{
	}
}
