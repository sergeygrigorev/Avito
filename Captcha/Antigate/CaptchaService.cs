using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;

namespace Captcha.Antigate
{
	public class CaptchaService : ICaptchaService
	{
		private const string resUrl = "http://antigate.com/res.php?key={0}&action=get&id={1}";
		private const string inUrl = "http://antigate.com/in.php";
		private const string inData = "method=base64&key={0}&body={1}";
		private const int interval = 5000;

		private string token;
		private string captcha = "";

		public CaptchaService(string token = "")
		{
			this.token = token;
		}
		
		public string GetCaptcha(Image image)
		{
			bool flag = true;
			int retries = 5;
			string id = "";

			MemoryStream ms = new MemoryStream();
			image.Save(ms,ImageFormat.Jpeg);
			string data = Convert.ToBase64String(ms.ToArray());
			data = WebUtility.UrlEncode(data);
			data = String.Format(inData, token, data);

			while (flag && retries > 0)
			{
				flag = false;
				WebClient web = new WebClient();
				web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				try
				{
					string res = web.UploadString(inUrl, data);
					if (res.Contains("OK|"))
						id = res.Substring(3);
					else throw new CaptchaErrorException();
				}
				catch (Exception)
				{
					retries--;
					flag = true;
				}
			}
			if (flag)
				return null;
			
			return GetResponse(id);
		}

		private string GetResponse(string id)
		{
			while (true)
			{
				CaptchaState state = Check(id);
				if (state == CaptchaState.Success)
					return captcha;
				else if (state == CaptchaState.NotReady)
					Thread.Sleep(interval);
				else return null;
			}
		}

		private CaptchaState Check(string id)
		{
			WebClient web = new WebClient();
			try
			{
				captcha = web.DownloadString(String.Format(resUrl, token, id));
			}
			catch(Exception e)
			{
				return CaptchaState.Error;
			}
			if (captcha.Contains("OK|"))
			{
				captcha = captcha.Substring(3);
				return CaptchaState.Success;
			}
			if (captcha.Contains("ERROR"))
				return CaptchaState.Error;
			return CaptchaState.NotReady;
		}

		public string GetToken()
		{
			return token;
		}
	}
}
