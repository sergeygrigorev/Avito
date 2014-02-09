using System.Collections.Generic;

namespace AvitoLibrary.Location
{
	interface ILocationService
	{
		UserLocation Default(bool reload = false);

		List<Region> GetRegions(bool reload = false);
		List<City> GetCities(int RegionId);
		List<District> GetDistricts(int CityId);
		List<Metro> GetMetros(int CityId);
		List<Road> GetRoads(int CityId);
	}
}
