using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AvitoLibrary;
using AvitoLibrary.Advert;
using AvitoLibrary.Location;
using AvitoLibrary.UserInfo;
using Captcha;
using Captcha.Antigate;

namespace AvitoConsole
{
	class lol : ICaptchaService
	{
		public string GetCaptcha(Image im)
		{
			return "123";
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Avito a = new Avito();
			a.Login(Settings.login, Settings.password);
			a.Init();
			a.SetCaptchaRecognizer(new CaptchaService(Settings.token));
			//a.SetCaptchaRecognizer(new lol());

			List<Advertisement> all = a.GetAll();
			User u = a.GetUserInfo();
			foreach (Advertisement x in all)
			{
				if (x.Id == 0)
					continue;
				x.User.Name = u.Name;
				x.User.Email = u.Email;
				try
				{
					a.Post(x);
				}
				catch(WrongAdvertisementException e)
				{
					Console.WriteLine("Ad Problems!");
					continue;
				}
				catch(CaptchaException e)
				{
					Console.WriteLine("Captcha Problems!");
					continue;
				}
				catch (Exception e)
				{
					Console.WriteLine("Other problems!");
					continue;
				}
				a.Close(x);
			}




			/*
			List<Region> reg = a.GetRegions();
			UserLocation loc = a.GetDefaultLocation();
			List<City> cities = a.GetCities(15);
			while (true)
			{
				Console.Write("> ");
				string command = Console.ReadLine();
				if (command == "exit")
					break;
				else if (command == "list")
					for (int i = 0; i < reg.Count; i++)
						Console.WriteLine("{0}. {1}", i, reg[i]);
				else if (command == "show")
				{
					if (cities == null)
						Console.WriteLine("Cities is null!");
					else foreach (City x in cities)
							Console.WriteLine(x);
				}
				else if (command.Substring(0,3) == "get")
					cities = a.GetCities(reg[Int32.Parse(command.Substring(4))].Id);
				else Console.WriteLine("Wrong command!");
			}*/
		}
	}
}
