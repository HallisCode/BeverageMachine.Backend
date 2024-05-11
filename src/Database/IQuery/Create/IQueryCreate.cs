using System.Threading.Tasks;

namespace Database.IQuery.Create
{
	public interface IQueryCreate<TEntity>
	{
		Task<TEntity> CreateAsync(TEntity entity);
	}
}
