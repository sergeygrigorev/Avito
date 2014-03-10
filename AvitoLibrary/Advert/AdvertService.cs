using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using AvitoLibrary.Auth;
using AvitoLibrary.Location;
using Captcha;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Region = AvitoLibrary.Location.Region;

namespace AvitoLibrary.Advert
{
	class AdvertService : IAdvertService
	{
		private const string baseUrl = "http://www.avito.ru";
		private const string baseUrlSecure = "https://www.avito.ru";
		private const string url1 = baseUrl + "/additem";
		private const string url2 = url1 + "/confirm";
		private const string imgUrl = url1 + "/image";
		private const string captchaUrl = baseUrl + "/captcha?";
		private const string itemsUrl = baseUrlSecure + "/profile/items";
		private const string oldItemsUrl = itemsUrl + "/old";
		private const string editUrl = baseUrlSecure + "/items/edit/";

		private const string closeMask = "item_id[]={0}&reason=other&delete=%D0%A1%D0%BD%D1%8F%D1%82%D1%8C+%D0%BE%D0%B1%D1%8A%D1%8F%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5+%D1%81+%D0%BF%D1%83%D0%B1%D0%BB%D0%B8%D0%BA%D0%B0%D1%86%D0%B8%D0%B8";
		private const string deleteMask = "remove=%D0%A3%D0%B4%D0%B0%D0%BB%D0%B8%D1%82%D1%8C+%D0%BD%D0%B0%D0%B2%D1%81%D0%B5%D0%B3%D0%B4%D0%B0&item_id[]={0}";

		private IAuthService auth;
		private ICaptchaService captcha;

		public AdvertService(IAuthService a = null, ICaptchaService c = null)
		{
			auth = a;
			captcha = c;
		}

		public Advertisement Get(string url)
		{
			string tmpurl = url;

			url = editUrl + url.Substring(url.IndexOf(".ru/") + 4).Replace('/', '_');

			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(web.DownloadString(url));
			HtmlNode node = doc.DocumentNode.SelectSingleNode("//form[@id='f_item']");
			HtmlNode tmp;

			Advertisement ad = new Advertisement();
			ad.User = new UserInfo.User();
			ad.Location = new UserLocation();
			ad.Parameters = new Dictionary<int, int>();
			ad.Images = new List<Image>();

			// Id
			ad.Id = Int32.Parse(url.Substring(url.LastIndexOf('_') + 1));

			// URL
			ad.Url = tmpurl;

			// Title
			ad.Title = "";

			// Body
			ad.Body = node.SelectSingleNode("//*[@name='description']").InnerText;

			// Name
			//ad.User.Name = "Частное лицо";

			// Manager
			ad.User.Manager = node.SelectSingleNode("//*[@name='manager']").GetAttributeValue("value", "");

			// Phone
			ad.User.Phone = node.SelectSingleNode("//*[@name='phone']").GetAttributeValue("value", "");

			// Allow Mails
			ad.User.DenyEmails = node.SelectSingleNode("//*[@name='allow_mails']").GetAttributeValue("checked", "xxx") == "xxx";

			// Email
			//ad.User.Email = "unstope@narod.ru";

			// Location
			// Region

			ad.Location.Region = new Region
			{
				Id = Int32.Parse(node.SelectSingleNode("//*[@id='region']").GetAttributeValue("value", "0"))
			};

			// City
			ad.Location.City = new City
			{
				Id = Int32.Parse(node.SelectSingleNode("//*[@name='location_id']").GetAttributeValue("value", "0"))
			};

			// Metro
			tmp = node.SelectSingleNode("//*[@name='metro_id']//*[@selected]");
			if (tmp != null)
				ad.Location.Metro = new Metro
										{
											Id = Int32.Parse(tmp.GetAttributeValue("value", "0"))
										};

			// Road
			tmp = node.SelectSingleNode("//*[@name='road_id']//*[@selected]");
			if (tmp != null)
				ad.Location.Road = new Road
				{
					Id = Int32.Parse(tmp.GetAttributeValue("value", "0"))
				};

			// District
			tmp = node.SelectSingleNode("//*[@name='district_id']//*[@selected]");
			if (tmp != null)
				ad.Location.District = new District
				{
					Id = Int32.Parse(tmp.GetAttributeValue("value", "0"))
				};

			// End Location

			// Category
			ad.CategoryId = Int32.Parse(node.SelectSingleNode("//*[@name='category_id']").GetAttributeValue("value", ""));

			// Parameters
			HtmlNodeCollection par = node.SelectNodes("//*[contains(@name,'params')]");
			foreach (HtmlNode x in par)
			{
				string id = x.GetAttributeValue("name", "").Substring(7);
				id = id.Substring(0, id.IndexOf(']'));
				int value = Int32.Parse(x.SelectSingleNode(".//*[@selected]").GetAttributeValue("value", ""));
				ad.Parameters.Add(Int32.Parse(id), value);
			}

			// Geo

			tmp = node.SelectSingleNode("//*[@name='coords[lat]']");
			if (tmp != null)
			{
				ad.Coordinates = new GeoCoords
				{
					Latitude = tmp.GetAttributeValue("value", ""),
					Longitude = node.SelectSingleNode("//*[@name='coords[lng]']").GetAttributeValue("value", ""),
					Zoom = node.SelectSingleNode("//*[@name='coords[zoom]']").GetAttributeValue("value", "")
				};
			}

			// Price
			ad.Price = Int32.Parse(node.SelectSingleNode("//*[@name='price']").GetAttributeValue("value", ""));

			// Images
			par = node.SelectSingleNode("//*[@name='images[]']").ParentNode.SelectNodes(".//img");
			foreach (HtmlNode x in par)
			{
				string[] arr = x.GetAttributeValue("src", "").Split('/');
				arr[3] = "640x480";
				byte[] data;
				try
				{
					data = web.DownloadData(String.Join("/", arr));
				}
				catch (Exception e)
				{
					return null;
				}
				MemoryStream ms = new MemoryStream();
				ms.Write(data, 0, data.Length);
				Image im = Image.FromStream(ms);
				ad.Images.Add(im);
			}

			// End

			return ad;
		}

		public List<Advertisement> GetActive()
		{
			List<Advertisement> list = new List<Advertisement>();

			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(web.DownloadString(itemsUrl));
			HtmlNodeCollection all = doc.DocumentNode.SelectNodes("//div[contains(@class,'item')]");

			if (all != null)
			{
				foreach (HtmlNode node in all)
				{
					Advertisement ad = new Advertisement();
					ad.Id = Int32.Parse(node.SelectSingleNode(".//input").GetAttributeValue("value", "0"));
					ad.Url = baseUrlSecure + node.SelectSingleNode(".//a[starts-with(@name,'item_')]").GetAttributeValue("href", "");
					ad.Title = node.SelectSingleNode(".//a[starts-with(@name,'item_')]").InnerText;
					ad = Get(ad.Url);
					list.Add(ad);
				}
			}

			return list;
		}

		public List<Advertisement> GetClosed()
		{
			List<Advertisement> list = new List<Advertisement>();

			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(web.DownloadString(oldItemsUrl));
			HtmlNodeCollection all = doc.DocumentNode.SelectNodes("//div[contains(@class,'item')]");

			if (all != null)
			{
				foreach (HtmlNode node in all)
				{
					Advertisement ad = new Advertisement();
					ad.Id = Int32.Parse(node.SelectSingleNode(".//input").GetAttributeValue("value", "0"));
					ad.Url = baseUrlSecure + node.SelectSingleNode(".//a[starts-with(@name,'item_')]").GetAttributeValue("href", "");
					ad.Title = node.SelectSingleNode(".//a[starts-with(@name,'item_')]").InnerText;
					ad = Get(ad.Url);
					list.Add(ad);
				}
			}

			return list;
		}

		public bool Close(int id)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Headers.Add("Referer", itemsUrl);
			web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			try
			{
				web.UploadString(itemsUrl, String.Format(closeMask, id.ToString()));
			}
			catch (Exception e)
			{
				return false;
			}

			return true;
		}

		public bool Delete(int id)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Headers.Add("Referer", oldItemsUrl);
			web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			try
			{
				web.UploadString(oldItemsUrl, String.Format(deleteMask, id.ToString()));
			}
			catch (Exception e)
			{
				return false;
			}

			return true;
		}

		public bool Post(Advertisement ad)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();

			data.Add("seller_name", ad.User.Name);
			data.Add("manager", (ad.User.Manager == null) ? "" : ad.User.Manager);
			data.Add("phone", ad.User.Phone);
			data.Add("email", ad.User.Email);
			data.Add("allow_mails", (ad.User.DenyEmails) ? "1" : "");

			data.Add("location_id", ad.Location.City.Id.ToString());
			data.Add("metro_id", (ad.Location.Metro == null) ? "" : ad.Location.Metro.Id.ToString());
			data.Add("district_id", (ad.Location.District == null) ? "" : ad.Location.District.Id.ToString());
			data.Add("road_id", (ad.Location.Road == null) ? "" : ad.Location.Road.Id.ToString());

			data.Add("category_id", ad.CategoryId.ToString());
			foreach (KeyValuePair<int, int> x in ad.Parameters)
			{
				data.Add("params[" + x.Key.ToString() + "]", x.Value.ToString());
			}

			data.Add("title", ad.Title);
			data.Add("description", ad.Body);

			if (ad.Coordinates != null)
			{
				data.Add("coords[lat]", ad.Coordinates.Latitude);
				data.Add("coords[lng]", ad.Coordinates.Longitude);
				data.Add("coords[zoom]", ad.Coordinates.Zoom);
			}

			data.Add("price", ad.Price.ToString());

			data.Add("service_code", "free");
			data.Add("main_form_submit", "Продолжить с пакетом «Обычная продажа»");

			foreach (Image image in ad.Images)
				data.Add("images[]", PostImage(image));

			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			web.Headers.Add("Referer", url1);

			try
			{
				web.UploadString(url1, Stringify(data));
			}
			catch (Exception ex)
			{
				throw new AvitoNetworkException();
			}
			if (!web.ResponseUrl.Contains("confirm"))
				throw new WrongAdvertisementException();

			string date = ((int)((DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds)).ToString();

			HttpWebRequest req = HttpWebRequest.Create(captchaUrl + date) as HttpWebRequest;
			req.Referer = url2;
			req.CookieContainer = auth.CredsCont;
			HttpWebResponse res;
			try
			{
				res = req.GetResponse() as HttpWebResponse;
			}
			catch (Exception ex)
			{
				return false;
			}
			Image im = new Bitmap(res.GetResponseStream());

			string cap;
			try
			{
				cap = captcha.GetCaptcha(im);
			}
			catch (Exception ex)
			{
				return false;
			}

			data.Clear();
			data.Add("captcha", cap);
			data.Add("service", "0");
			data.Add("subscribe-position", "0");

			web.Headers.Add("Referer", url2);
			web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

			try
			{
				web.UploadString(url2, Stringify(data));
			}
			catch (Exception ex)
			{
				throw new AvitoNetworkException();
			}
			if (!web.ResponseUrl.Contains("finish"))
				throw new CaptchaException();
			return true;
		}

		private string Stringify(IDictionary dict)
		{
			StringBuilder s = new StringBuilder();

			foreach (DictionaryEntry x in dict)
			{
				if (s.Length > 0)
					s.Append("&");
				string key = x.Key.ToString();
				string val = (x.Value == null) ? "" : x.Value.ToString();
				s.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(val));
			}

			return s.ToString();
		}

		private string PostImage(Image im)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Headers.Add("Content-Type", "multipart/form-data; boundary=bound");
			web.Headers.Add("Referer", url1);

			List<byte> data = new List<byte>();
			MemoryStream str = new MemoryStream();
			im.Save(str, ImageFormat.Jpeg);
			StringBuilder s1 = new StringBuilder();
			s1.AppendLine("--bound");
			s1.AppendLine("Content-Disposition: form-data; name=\"image\"; filename=\"1.jpg\"");
			s1.AppendLine("Content-Type: image/jpeg");
			s1.AppendLine();
			StringBuilder s2 = new StringBuilder();
			s2.AppendLine("\n--bound");
			data.AddRange(Encoding.UTF8.GetBytes(s1.ToString()));
			data.AddRange(str.ToArray());
			data.AddRange(Encoding.UTF8.GetBytes(s2.ToString()));

			string res = "";
			try
			{
				byte[] arr = web.UploadData(imgUrl, data.ToArray());
				res = Encoding.UTF8.GetString(arr);
			}
			catch (Exception e)
			{
				throw new WebException("Error making image posting\n");
			}

			JObject jo = JObject.Parse(res);
			return jo["id"].ToString();
		}

		public bool Bump(string url)
		{
			throw new NotImplementedException();
		}
	}
}
