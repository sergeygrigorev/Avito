using System;
using System.Net;

namespace AvitoLibrary
{
	class AvitoWebClient : WebClient
	{
		private CookieContainer container;
		private string url;

		public AvitoWebClient(CookieContainer container)
		{
			this.container = container;
		}

		public CookieContainer Container
		{
			get { return container; }
		}

		public string ResponseUrl
		{
			get { return url; }
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest r = base.GetWebRequest(address);
			var request = r as HttpWebRequest;
			if (request != null)
			{
				request.CookieContainer = container;
			}
			return r;
		}

		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse response = base.GetWebResponse(request, result);
			var r = response as HttpWebResponse;
			if (r != null)
			{
				CookieCollection cookies = r.Cookies;
				container.Add(cookies);
				url = r.ResponseUri.ToString();
			}
			return response;
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = base.GetWebResponse(request);
			var r = response as HttpWebResponse;
			if (r != null)
			{
				CookieCollection cookies = r.Cookies;
				container.Add(cookies);
				url = r.ResponseUri.ToString();
			}
            return response;
		}
	}
}
