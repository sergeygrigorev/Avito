using System;

namespace Captcha.Antigate
{
	class NoFreeWorkerException : Exception
	{
		public override string Message
		{
			get { return "There are no free workers available, try later or rise payment\r\n"; }
		}
	}
}
