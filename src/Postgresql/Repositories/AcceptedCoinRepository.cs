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
	public class AcceptedCoinRepository : IAcceptedCoinRepository
	{
		private ApplicationDbContext _dbContext;


		public AcceptedCoinRepository(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}


		public async Task<int> CountAsync(Expression<Func<AcceptedCoin, bool>> predicate)
		{
			return await _dbContext.AcceptedCoins.CountAsync(predicate);
		}

		public async Task<AcceptedCoin> CreateAsync(AcceptedCoin entity)
		{
			_dbContext.AcceptedCoins.Add(entity);

			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task CreateAsync(IEnumerable<AcceptedCoin> entities)
		{
			_dbContext.AcceptedCoins.AddRange(entities);

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(AcceptedCoin entity)
		{
			_dbContext.AcceptedCoins.Remove(entity);

			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(IEnumerable<AcceptedCoin> entities)
		{
			_dbContext.AcceptedCoins.RemoveRange(entities);

			await _dbContext.SaveChangesAsync();
		}

		public Task<List<AcceptedCoin>> SelectAllAsync(Expression<Func<AcceptedCoin, bool>> predicate, int take, int skip)
		{
			return _dbContext.AcceptedCoins.Where(predicate).Skip(skip).Take(take).ToListAsync();
		}

		public Task<AcceptedCoin> SelectFirstAsync(Expression<Func<AcceptedCoin, bool>> predicate)
		{
			return _dbContext.AcceptedCoins.FirstOrDefaultAsync(predicate);
		}

		public async Task UpdateAsync(AcceptedCoin entity)
		{
			_dbContext.AcceptedCoins.Update(entity);

			await _dbContext.SaveChangesAsync();
		}
	}
}
