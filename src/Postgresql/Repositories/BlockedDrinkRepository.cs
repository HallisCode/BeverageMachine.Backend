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
	public class BlockedDrinkRepository : IBlockedDrinkRepository
	{
		private ApplicationDbContext _dbContext;


		public BlockedDrinkRepository(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}

		public async Task<BlockedDrink> CreateAsync(BlockedDrink entity)
		{
			_dbContext.BlockedDrinks.Add(entity);

			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task DeleteAsync(BlockedDrink entity)
		{
			_dbContext.BlockedDrinks.Remove(entity);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<BlockedDrink>> SelectAllAsync(Expression<Func<BlockedDrink, bool>> predicate)
		{
			return await _dbContext.BlockedDrinks.Where(predicate).ToListAsync();
		}

		public async Task<BlockedDrink> SelectFirstAsync(Expression<Func<BlockedDrink, bool>> predicate)
		{
			return await _dbContext.BlockedDrinks.FirstOrDefaultAsync(predicate);
		}

		public async Task UpdateAsync(BlockedDrink entity)
		{
			_dbContext.BlockedDrinks.Update(entity);

			await _dbContext.SaveChangesAsync();
		}
	}
}
