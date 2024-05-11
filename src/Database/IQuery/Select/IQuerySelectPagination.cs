using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database.IQuery.Select
{
	public interface IQuerySelectPagination<TEntity>
	{
		Task<List<TEntity>> SelectAllAsync(Expression<Func<TEntity, bool>> predicate, int take, int skip);
	}
}
