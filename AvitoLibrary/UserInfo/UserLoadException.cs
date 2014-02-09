using System;

namespace AvitoLibrary.UserInfo
{
	class UserLoadException : Exception
	{
		public override string Message
		{
			get
			{
				return "Error loading user info\r\n";
			}
		}
	}
}
