using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvitoLibrary.Advert
{
	interface IAdvertService
	{
		Advertisement Get(string url);
		List<Advertisement> GetActive();
		List<Advertisement> GetClosed();
		bool Close(int id);
		bool Delete(int id);
		bool Post(Advertisement ad);
	    bool Bump(string url);
	}
}
