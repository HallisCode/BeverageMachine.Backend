using System;

namespace Domain.Exceptions
{
	public class DetailedException : Exception
	{
		public object Details { get; init; }

		public DetailedException(string message, object details = null) : base(message)
			=> Details = details;

		public DetailedException(string message, Exception innerException, object details = null) : base(message, innerException)
			=> Details = details;
	}
}
