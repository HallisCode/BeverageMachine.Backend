using Domain.Exceptions;
using System;


namespace Application.Exceptions
{
	public class ApplicationLayerException : DetailedException
	{
		public ApplicationLayerException(string message, object details = null) : base(message, details) { }

		public ApplicationLayerException(string message, Exception innerException, object details = null) : base(message, innerException, details) { }
	}
}
