using System;
using System.Net;

namespace AvitoLibrary.Auth
{
	class AuthService : IAuthService
	{
		private CookieCollection creds;

		public AuthService()
		{
			creds = new CookieCollection();
		}

		public bool Login(string login, string password)
		{
			WebClientEx web = new WebClientEx(new CookieContainer());

			web.Headers.Add("Content-Type", "multipart/form-data; boundary=bound");
			web.Headers.Add("Referer", "https://www.avito.ru/profile/login?next=%2Fprofile");
			string data = "--bound\n" +
							"Content-Disposition: form-data; name=\"next\"\n" +
							"\n" +
							"/profile\n" +
							"--bound\n" +
							"Content-Disposition: form-data; name=\"login\"\n" +
							"\n" +
							"unstope@narod.ru\n" +
							"--bound\n" +
							"Content-Disposition: form-data; name=\"password\"\n" +
							"\n" +
							"kirillov4ever\n" +
							"--bound\n";

			try
			{
				web.UploadString("https://www.avito.ru/profile/login", data);
			}
			catch(Exception e)
			{
				return false;
			}
			
			creds = web.Container.GetCookies(new Uri("http://www.avito.ru"));

			return true;
		}

		public CookieCollection Creds
		{
			get { return creds; }
		}

		public CookieContainer CredsCont
		{
			get
			{
				if (creds == null)
					return null;
				CookieContainer c = new CookieContainer();
				c.Add(creds);
				return c;
			}
		}
	}
}
