using System;

namespace AvitoLibrary.Location
{
	class LocationLoadException : Exception
	{
		public override string Message
		{
			get { return "Error loading location\r\n"; }
		}
	}
}
