using System.IO;
using System.Net;
using System.Text;
using AvitoLibrary.Auth;

namespace AvitoLibrary
{
	class GlobalParseService
	{
		private static IAuthService auth;

		private const string AddItemUrl = "http://www.avito.ru/additem";

		private static string newAdHtml = "";
		private static string catJson = "";
		private static string profileSettingsHtml = "";

		public static void Init()
		{
			CookieContainer cookies = new CookieContainer();
			cookies.Add(auth.Creds);
			AvitoWebClient web = new AvitoWebClient(cookies);
			web.Encoding = Encoding.UTF8;
			string data = web.DownloadString(AddItemUrl);
			newAdHtml = data;

			data = data.Substring(data.LastIndexOf("<script"));
			data = data.Substring(data.IndexOf('\"') + 1);
			data = data.Substring(0, data.IndexOf('\"'));
			data = "http:" + data;
			HttpWebRequest r = WebRequest.Create(data) as HttpWebRequest;
			r.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			HttpWebResponse rs = r.GetResponse() as HttpWebResponse;
			StreamReader sr = new StreamReader(rs.GetResponseStream());
			data = sr.ReadToEnd();
			data = data.Substring(data.IndexOf("avito.filters="));
			data = data.Substring(data.IndexOf('{'));
			int i, num;
			for (i = 0, num = 0; i < data.Length; i++)
			{
				if (data[i] == '{')
					num++;
				if (data[i] == '}')
					num--;
				if (num == 0)
					break;
			}
			data = data.Substring(0, i + 1);
			catJson = data;
		}

		public static void SetAuth(IAuthService a)
		{
			auth = a;
		}

		public static string NewAdHtml
		{
			get { return newAdHtml; }
		}

		public static string ProfileSettingsHtml
		{
			get { return profileSettingsHtml; }
		}

		public static string CatJson
		{
			get { return catJson; }
		}
	}
}
