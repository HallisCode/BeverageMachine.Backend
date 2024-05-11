using Database.IQuery.Select;
using Domain.Entities;
using System.Threading.Tasks;

namespace Database.IRepositories
{
	public interface IDrinkRepository :
		IBaseOperations<Drink>, IQuerySelectAll<Drink>
	{
		Task<Drink> SelectFirstLikeTitleAsync(string pattern);
	}
}
