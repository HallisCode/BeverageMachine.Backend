using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database.IQuery.Create
{
	public interface IQueryCreateRange<TEntity>
	{
		Task CreateAsync(IEnumerable<TEntity> entities);
	}
}
