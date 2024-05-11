using System;


namespace LocalImages.Exceptions
{
	public class LocalImagesOptionsConfugeredException : Exception
	{
		public LocalImagesOptionsConfugeredException(string message) : base(message)
		{
		}

		public LocalImagesOptionsConfugeredException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
