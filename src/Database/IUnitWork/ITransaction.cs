using System;
using System.Threading.Tasks;

namespace Database.IUnitWork
{
	public interface ITransaction : IDisposable
	{
		public Task RollBackAsync();

		public Task CommitAsync();
	}
}
