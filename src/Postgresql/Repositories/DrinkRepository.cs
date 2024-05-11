using Database.IRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Postgresql.Repositories
{
	public class DrinkRepository : IDrinkRepository
	{
		private ApplicationDbContext _dbContext;


		public DrinkRepository(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}


		public async Task<Drink> CreateAsync(Drink entity)
		{
			_dbContext.Add(entity);

			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task DeleteAsync(Drink entity)
		{
			_dbContext.Remove(entity);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<Drink>> SelectAllAsync(Expression<Func<Drink, bool>> predicate)
		{
			return await _dbContext.Drinks.Where(predicate).ToListAsync();
		}

		public async Task<Drink> SelectFirstAsync(Expression<Func<Drink, bool>> predicate)
		{
			return await _dbContext.Drinks.FirstOrDefaultAsync(predicate);
		}

		public async Task<Drink> SelectFirstLikeTitleAsync(string pattern)
		{
			return await _dbContext.Drinks.FirstOrDefaultAsync(drink => EF.Functions.ILike(drink.Title, pattern));
		}

		public async Task UpdateAsync(Drink entity)
		{
			_dbContext.Update(entity);

			await _dbContext.SaveChangesAsync();
		}
	}
}
