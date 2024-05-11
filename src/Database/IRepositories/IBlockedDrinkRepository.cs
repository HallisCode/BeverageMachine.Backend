using Database.IQuery.Select;
using Domain.Entities;

namespace Database.IRepositories
{
	public interface IBlockedDrinkRepository : IBaseOperations<BlockedDrink>, IQuerySelectAll<BlockedDrink>
	{
	}
}
