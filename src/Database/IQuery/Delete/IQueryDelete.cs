using System.Threading.Tasks;

namespace Database.IQuery.Delete
{
	public interface IQueryDelete<TEntity>
	{
		Task DeleteAsync(TEntity entity);
	}
}
