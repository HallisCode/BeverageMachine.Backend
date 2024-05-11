using System;

namespace Application.Exceptions.CRUD
{
	internal class AlreadyExistException : ApplicationLayerException
	{
		public AlreadyExistException(string message) : base(message)
		{
		}

		public AlreadyExistException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
