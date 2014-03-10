using System;
using System.Collections.Generic;
using System.Text;
using AvitoLibrary.Auth;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace AvitoLibrary.Location
{
	class LocationService : ILocationService
	{
		private const string settingsUrl = "https://www.avito.ru/profile/settings";
		private const string regionsUrl = "http://www.avito.ru/js/locations?json=1";
		private const string citiesUrl = "http://www.avito.ru/js/locations?json=1&id={0}";
		private const string metrosUrl = "http://www.avito.ru/js/metro?locid={0}";
		private const string districtsUrl = "http://www.avito.ru/js/district?locid={0}";
		private const string roadsUrl = "http://www.avito.ru/js/road?locid={0}";

		private UserLocation def;
		private IAuthService auth;
		private List<Region> regions;

		public LocationService(IAuthService auth)
		{
			this.auth = auth;
		}

		public UserLocation Default(bool reload = false)
		{
			if (!reload && def != null)
				return def;
			def = new UserLocation();
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			HtmlDocument doc = new HtmlDocument();
			try
			{
				doc.LoadHtml(web.DownloadString(settingsUrl));
			}
			catch(Exception e)
			{
				def = null;
				throw new LocationLoadException();
			}

			HtmlNode tmp;

			// City

			tmp = doc.DocumentNode.SelectSingleNode("//*[@id='region']/option[@selected]");
			if (tmp != null)
			{
				def.City = new City
					           {
						           Id = Int32.Parse(tmp.GetAttributeValue("value", "")),
								   Name = tmp.NextSibling.InnerText
					           };
			}

			// Metro
			tmp = doc.DocumentNode.SelectSingleNode("//*[@id='metro_id']/option[@selected]");
			if (tmp != null)
			{
				def.Metro = new Metro
				{
					Id = Int32.Parse(tmp.GetAttributeValue("value", "")),
					Name = tmp.NextSibling.InnerText
				};
			}

			// District
			tmp = doc.DocumentNode.SelectSingleNode("//*[@id='district_id']/option[@selected]");
			if (tmp != null)
			{
				def.District = new District
				{
					Id = Int32.Parse(tmp.GetAttributeValue("value", "")),
					Name = tmp.NextSibling.InnerText
				};
			}

			return def;
		}

		public List<Region> GetRegions(bool reload = false)
		{
			if (!reload && regions != null)
				return regions;
			regions = new List<Region>();
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			string json = "";
			try
			{
				json = web.DownloadString(regionsUrl);
			}
			catch (Exception e)
			{
				regions = null;
				throw new LocationLoadException();
			}
			JArray arr = JArray.Parse(json);
			foreach (JObject x in arr)
			{
				Region r = new Region();
				r.Id = Int32.Parse(x["id"].ToString());
				r.Name = x["name"].ToString();
				regions.Add(r);
			}

			return regions.Count == 0 ? null : regions;
		}

		public List<City> GetCities(int RegionId)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			string json = "";
			try
			{
				json = web.DownloadString(String.Format(citiesUrl,RegionId));
			}
			catch (Exception e)
			{
				throw new LocationLoadException();
			}
			JArray arr;
			try
			{
				arr = JArray.Parse(json);
			}
			catch(Exception e)
			{
				return null;
			}
			List<City> cities = new List<City>();
			foreach (JObject x in arr)
			{
				City c = new City();
				c.Id = Int32.Parse(x["id"].ToString());
				c.Name = x["name"].ToString();
				cities.Add(c);
			}

			return cities.Count == 0 ? null : cities;
		}

		public List<Metro> GetMetros(int CityId)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			string json = "";
			try
			{
				json = web.DownloadString(String.Format(metrosUrl, CityId));
			}
			catch (Exception e)
			{
				throw new LocationLoadException();
			}
			JArray arr;
			try
			{
				arr = JArray.Parse(json);
			}
			catch (Exception e)
			{
				return null;
			}
			List<Metro> metros = new List<Metro>();
			foreach (JObject x in arr)
			{
				Metro m = new Metro();
				m.Id = Int32.Parse(x["id"].ToString());
				m.Name = x["name"].ToString();
				metros.Add(m);
			}

			return metros.Count == 0 ? null : metros;
		}

		public List<District> GetDistricts(int CityId)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			string json = "";
			try
			{
				json = web.DownloadString(String.Format(districtsUrl, CityId));
			}
			catch (Exception e)
			{
				throw new LocationLoadException();
			}
			JArray arr;
			try
			{
				arr = JArray.Parse(json);
			}
			catch (Exception e)
			{
				return null;
			}
			List<District> districts = new List<District>();
			foreach (JObject x in arr)
			{
				District d = new District();
				d.Id = Int32.Parse(x["id"].ToString());
				d.Name = x["name"].ToString();
				districts.Add(d);
			}

			return districts.Count == 0 ? null : districts;
		}

		public List<Road> GetRoads(int CityId)
		{
			AvitoWebClient web = new AvitoWebClient(auth.CredsCont);
			web.Encoding = Encoding.UTF8;
			string json = "";
			try
			{
				json = web.DownloadString(String.Format(roadsUrl, CityId));
			}
			catch (Exception e)
			{
				throw new LocationLoadException();
			}
			JArray arr;
			try
			{
				arr = JArray.Parse(json);
			}
			catch (Exception e)
			{
				return null;
			}
			List<Road> roads = new List<Road>();
			foreach (JObject x in arr)
			{
				Road r = new Road();
				r.Id = Int32.Parse(x["id"].ToString());
				r.Name = x["name"].ToString();
				roads.Add(r);
			}

			return roads.Count == 0 ? null : roads;
		}
	}
}
