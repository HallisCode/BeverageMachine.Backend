using Database.IUnitWork;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Postgresql.UnitWork
{
	public class Transaction : ITransaction
	{
		private bool disposed = false;

		private IDbContextTransaction transaction;


		public Transaction(IDbContextTransaction transaction)
		{
			this.transaction = transaction;
		}

		public async Task CommitAsync()
		{
			await transaction.CommitAsync();
		}

		public async Task RollBackAsync()
		{
			await transaction.RollbackAsync();
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					transaction.Dispose();
				}

				disposed = true;
			}
		}

		~Transaction()
		{
			Dispose(false);
		}
	}
}
