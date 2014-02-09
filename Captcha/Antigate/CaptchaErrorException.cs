using System;

namespace Captcha.Antigate
{
	class CaptchaErrorException : Exception
	{
		public override string Message
		{
			get
			{
				return "Error sending captcha\r\n";
			}
		}
	}
}
