using Database.IQuery.Create;
using Database.IQuery.Delete;
using Database.IQuery.Select;
using Database.IQuery.Update;

namespace Database.IRepositories
{
	public interface IBaseOperations<TEntity> :
		IQueryCreate<TEntity>, IQueryUpdate<TEntity>,
		IQueryDelete<TEntity>, IQuerySelectFirst<TEntity>
	{
	}
}
