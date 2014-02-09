using System.Collections.Generic;
using System.Threading.Tasks;
using AvitoLibrary.Advert;
using AvitoLibrary.Category;
using AvitoLibrary.Location;
using AvitoLibrary.UserInfo;
using Captcha;

namespace AvitoLibrary
{
	interface IAvito
	{
		bool Login(string login, string password);
		Task<bool> LoginAsync(string login, string password);

		void SetCaptchaRecognizer(ICaptchaService captcha);

		Advertisement Get(string url);
		Advertisement Get(int id);
		List<Advertisement> GetAll();

		bool Post(Advertisement ad);
		Task<bool> PostAsync(Advertisement ad);

		bool Close(int id);
		bool Close(Advertisement ad);
		bool Delete(int id);
		bool Delete(Advertisement ad);

		List<CategoryGroup> GetCat();
		List<Parameter> GetParam(int catId);
		User GetUserInfo();

		UserLocation GetDefaultLocation();
		List<Region> GetRegions();
		List<City> GetCities(int regionId);
		List<Metro> GetMetro(int cityId);
		List<District> GetDistricts(int cityId);
		List<Road> GetRoads(int cityId);

		bool Available();
		
		void Logout();
	}
}
