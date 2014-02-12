using System;
using System.Text;
using AvitoLibrary.Auth;
using HtmlAgilityPack;

namespace AvitoLibrary.UserInfo
{
	class UserService : IUserService
	{
		private const string settingsUrl = "https://www.avito.ru/profile/settings";

		private User def;
		private IAuthService auth;

		public UserService(IAuthService auth)
		{
			this.auth = auth;
		}

		public User GetDefault(bool reload = false)
		{
			if (!reload && def != null)
				return def;
			def = new User();
			WebClientEx web = new WebClientEx(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			HtmlDocument doc = new HtmlDocument();
			try
			{
				doc.LoadHtml(web.DownloadString(settingsUrl));
			}
			catch(Exception e)
			{
				def = null;
				throw new UserLoadException();
			}

			string tmp;

			// Id
			def.Id = Int32.Parse(doc.DocumentNode.SelectSingleNode("//*[@id='profile']/div[1]/div[2]/div[3]/div[1]/div[2]/span").InnerText);

			// Name
			def.Name = doc.DocumentNode.SelectSingleNode("//*[@id='fld_name']").GetAttributeValue("value", "");

			// Manager
			def.Manager = doc.DocumentNode.SelectSingleNode("//*[@id='fld_manager']").GetAttributeValue("value", "");

			// Phone
			def.Phone = doc.DocumentNode.SelectSingleNode("//*[@id='fld_phone']").GetAttributeValue("value", "");

			// Email
			tmp = doc.DocumentNode.SelectSingleNode("//*[@id='profile']/div[1]/div[2]/div[3]/div[1]/div[1]/span").InnerText;
			tmp = tmp.Substring(0, tmp.IndexOf(' '));
			def.Email = tmp;

			// Deny emails
			// TODO: get from local settings

			return def;
		}
	}
}
