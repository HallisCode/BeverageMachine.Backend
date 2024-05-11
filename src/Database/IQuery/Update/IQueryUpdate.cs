using System.Threading.Tasks;

namespace Database.IQuery.Update
{
	public interface IQueryUpdate<TEntity>
	{
		Task UpdateAsync(TEntity entity);
	}
}
