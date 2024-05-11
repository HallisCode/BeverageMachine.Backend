using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database.IQuery
{
	public interface IQueryCount<TEntity>
	{
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
	}
}
