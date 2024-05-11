using System;

namespace REST.Exceptions
{
	public class ValidationException : PresentationLayerException
	{
		public ValidationException(string message, object details = null) : base(message, details) { }

		public ValidationException(string message, Exception innerException, object details = null) : base(message, innerException, details) { }
	}
}
