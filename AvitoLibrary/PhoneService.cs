using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace AvitoLibrary
{
	class PhoneService
	{
		public PhoneService()
		{

		}

		public Image GetImage(string url)
		{
			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			Image im = new Bitmap(response.GetResponseStream());
			return im;
		}

		public void GetPhone(string url)
		{
			WebClient web = new WebClient();

			string response = web.DownloadString(url);
			int urlpos = response.IndexOf("item_url") + 12;
			int phonepos = response.IndexOf("item_phone") + 14;
			string adress = response.Substring(urlpos);
			string phonekey = response.Substring(phonepos);
			adress = adress.Substring(0, adress.IndexOf('\''));
			phonekey = phonekey.Substring(0, phonekey.IndexOf('\''));
			int id = response.IndexOf("\"item_id\"") + 10;
			string itemid = response.Substring(id);
			itemid = itemid.Substring(0, itemid.IndexOf('<'));
			id = Int32.Parse(itemid);

			phonekey = Demixer(phonekey, id);

			string ololo = "http://www.avito.ru/items/phone/" + adress + "?pkey=" + phonekey;

			web.Headers.Add("Referer", url);
			web.Headers.Add("Accept", "image/webp,*/*;q=0.8");
			web.DownloadFile(ololo,"image.png");

			return;
		}

		public string Demixer(string key, int id)
		{
			int k, s;
			string r = "";
			Regex regex = new Regex(@"[0-9a-f]+");
			List<string> pre = new List<string>();
			MatchCollection m = regex.Matches(key);

			foreach (Match x in m)
			{
				for (int i = 0; i < x.Groups.Count;i++ )
				{
					pre.Add(x.Groups[i].Value);
				}
			}


			if (id % 2 == 0)
				pre.Reverse();
			string mixed = "";
			for (int i = 0; i < pre.Count; i++)
				mixed += pre[i];
			s = mixed.Length;

			for (k = 0; k < s; k+=3)
					r += mixed.Substring(k, 1);

			return r;
		}
	}
}
