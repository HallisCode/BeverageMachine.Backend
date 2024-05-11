using System;

namespace Application.Exceptions.CRUD
{
	public class DoesNotExist : ApplicationLayerException
	{
		public DoesNotExist(string message) : base(message)
		{
		}

		public DoesNotExist(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
