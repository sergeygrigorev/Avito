using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvitoLibrary.Location
{
	public class GeoCoords
	{
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string Zoom { get; set; }

		public override string ToString()
		{
			return String.Format("[{0}, {1}], {2}", Latitude, Longitude, Zoom);
		}
	}
}
