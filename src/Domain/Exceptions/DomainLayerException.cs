using System;


namespace Domain.Exceptions
{
	public class DomainLayerException : DetailedException
	{
		public DomainLayerException(string message, object details = null) : base(message, details) { }

		public DomainLayerException(string message, Exception innerException, object details = null) : base(message, innerException, details) { }
	}
}
