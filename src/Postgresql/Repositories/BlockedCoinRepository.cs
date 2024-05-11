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
	public class BlockedCoinRepository : IBlockedCoinRepository
	{
		private ApplicationDbContext _dbContext;


		public BlockedCoinRepository(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}


		public async Task<BlockedCoin> CreateAsync(BlockedCoin entity)
		{
			_dbContext.BlockedCoins.Add(entity);

			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task DeleteAsync(BlockedCoin entity)
		{
			_dbContext.BlockedCoins.Remove(entity);

			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<BlockedCoin>> SelectAllAsync(Expression<Func<BlockedCoin, bool>> predicate)
		{
			return await _dbContext.BlockedCoins.Where(predicate).ToListAsync();
		}

		public async Task<BlockedCoin> SelectFirstAsync(Expression<Func<BlockedCoin, bool>> predicate)
		{
			return await _dbContext.BlockedCoins.FirstOrDefaultAsync(predicate);
		}

		public async Task UpdateAsync(BlockedCoin entity)
		{
			_dbContext.BlockedCoins.Update(entity);

			await _dbContext.SaveChangesAsync();
		}
	}
}
