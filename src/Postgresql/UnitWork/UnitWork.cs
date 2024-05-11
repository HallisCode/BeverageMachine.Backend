using Database.IRepositories;
using Database.IUnitWork;
using Postgresql.Repositories;
using System.Threading.Tasks;

namespace Postgresql.UnitWork
{
	public class UnitWork : IUnitWork
	{
		private ApplicationDbContext _dbContext;


		private IAcceptedCoinRepository _acceptedCoinRepository;

		private IDrinkRepository _drinkRepository;

		private IBlockedCoinRepository _blockedCoinRepository;

		private IBlockedDrinkRepository _blockedDrinkRepository;


		public IAcceptedCoinRepository AcceptedCoinRepository
		{
			get
			{
				if (_acceptedCoinRepository is null) _acceptedCoinRepository = new AcceptedCoinRepository(_dbContext);

				return _acceptedCoinRepository;
			}
		}

		public IDrinkRepository DrinkRepository
		{
			get
			{
				if (_drinkRepository is null) _drinkRepository = new DrinkRepository(_dbContext);

				return _drinkRepository;
			}
		}

		public IBlockedCoinRepository BlockedCoinRepository
		{
			get
			{
				if (_blockedCoinRepository is null) _blockedCoinRepository = new BlockedCoinRepository(_dbContext);

				return _blockedCoinRepository;
			}
		}

		public IBlockedDrinkRepository BlockedDrinkRepository
		{
			get
			{
				if (_blockedDrinkRepository is null) _blockedDrinkRepository = new BlockedDrinkRepository(_dbContext);

				return _blockedDrinkRepository;
			}
		}


		public UnitWork(ApplicationDbContext dbContext)
		{
			this._dbContext = dbContext;
		}


		public async Task<ITransaction> BeginTransactionAsync()
		{
			return new Transaction(await _dbContext.Database.BeginTransactionAsync());
		}
	}
}
