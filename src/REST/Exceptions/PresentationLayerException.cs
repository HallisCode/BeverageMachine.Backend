using Domain.Exceptions;
using System;

namespace REST.Exceptions
{
	public class PresentationLayerException : DetailedException
	{
		public PresentationLayerException(string message, object details = null) : base(message, details) { }

		public PresentationLayerException(string message, Exception innerException, object details = null) : base(message, innerException, details) { }
	}
}
