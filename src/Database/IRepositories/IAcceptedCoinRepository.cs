using Database.IQuery;
using Database.IQuery.Create;
using Database.IQuery.Delete;
using Database.IQuery.Select;
using Domain.Entities;

namespace Database.IRepositories
{
	public interface IAcceptedCoinRepository :
		IBaseOperations<AcceptedCoin>, IQuerySelectPagination<AcceptedCoin>,
		IQueryCount<AcceptedCoin>, IQueryCreateRange<AcceptedCoin>,
		IQueryDeleteAll<AcceptedCoin>
	{
	}
}
