using System;

namespace Domain.Exceptions
{
	public class UnSpecifiedCoinException : DomainLayerException
	{
		public UnSpecifiedCoinException(string message, object details = null) : base(message, details) { }

		public UnSpecifiedCoinException(string message, Exception innerException, object details = null) : base(message, innerException, details) { }

	}
}
