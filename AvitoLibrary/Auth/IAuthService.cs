using System.Net;

namespace AvitoLibrary.Auth
{
	interface IAuthService
	{
		bool Login(string login, string password);

		CookieCollection Creds { get; }
		CookieContainer CredsCont { get; }
	}
}
