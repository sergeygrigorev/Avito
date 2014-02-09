using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AvitoLibrary.Advert;
using AvitoLibrary.Auth;
using AvitoLibrary.Category;
using AvitoLibrary.Location;
using AvitoLibrary.UserInfo;
using Captcha;

namespace AvitoLibrary
{
	public class Avito : IAvito
	{
		private const string pingHost = "www.avito.ru";

		private IAuthService auth;
		private ICaptchaService captcha;
		private List<CategoryGroup> catg;

		public void Init()
		{
			GlobalParseService.SetAuth(auth);
			GlobalParseService.Init();
		}

		public void SetCaptchaRecognizer(ICaptchaService captcha)
		{
			this.captcha = captcha;
		}

		public bool Login(string login, string password)
		{
			auth = new AuthService();
			return auth.Login(login, password);
		}

		public async Task<bool> LoginAsync(string login, string password)
		{
			throw new NotImplementedException();
		}

		public void Logout()
		{
			auth = null;
		}

		public bool Available()
		{
			Ping p = new Ping();
			PingReply r = p.Send(pingHost, 1000);
			if (r.Status == IPStatus.Success)
				return true;
			return false;
		}

		public Advertisement Get(string url)
		{
			AdvertService ad = new AdvertService(auth);
			return ad.Get(url);
		}

		public Advertisement Get(int id)
		{
			throw new NotImplementedException();
		}

		public bool Post(Advertisement ad)
		{
			AdvertService ads = new AdvertService(auth, captcha);

			return ads.Post(ad);
		}

		public Task<bool> PostAsync(Advertisement ad)
		{
			throw new NotImplementedException();
		}

		public List<Advertisement> GetAll()
		{
			AdvertService ad = new AdvertService(auth);
			return ad.GetAll();
		}

		public bool Close(int id)
		{
			AdvertService ad = new AdvertService(auth);
			return ad.Close(id);
		}

		public bool Close(Advertisement ad)
		{
			return Close(ad.Id);
		}

		public bool Delete(int id)
		{
			AdvertService ad = new AdvertService(auth);
			return ad.Delete(id);
		}

		public bool Delete(Advertisement ad)
		{
			return Delete(ad.Id);
		}

		public List<CategoryGroup> GetCat()
		{
			if (catg == null)
			{
				CategoryService cats = new CategoryService();
				catg = cats.GetCategories();
			}
			return catg;
		}

		public List<Parameter> GetParam(int catId)
		{
			List<Parameter> par;
			try
			{
				par = catg.Where(p => p.Categories.Count(q => q.Id == catId) > 0).Select(p => p.Categories.First(q => q.Id == catId).Parameters).First();
			}
			catch (Exception e)
			{
				return null;
			}

			return par;
		}

		public User GetUserInfo()
		{
			UserService s = new UserService(auth);
			return s.Default();
		}

		public UserLocation GetDefaultLocation()
		{
			LocationService loc = new LocationService(auth);
			return loc.Default();
		}

		public List<Region> GetRegions()
		{
			LocationService loc = new LocationService(auth);
			return loc.GetRegions();
		}

		public List<City> GetCities(int regionId)
		{
			LocationService loc = new LocationService(auth);
			return loc.GetCities(regionId);
		}

		public List<Metro> GetMetro(int cityId)
		{
			LocationService loc = new LocationService(auth);
			return loc.GetMetros(cityId);
		}

		public List<District> GetDistricts(int cityId)
		{
			LocationService loc = new LocationService(auth);
			return loc.GetDistricts(cityId);
		}

		public List<Road> GetRoads(int cityId)
		{
			LocationService loc = new LocationService(auth);
			return loc.GetRoads(cityId);
		}
	}
}
