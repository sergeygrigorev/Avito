using System.Drawing;

namespace Captcha
{
	public interface ICaptchaService
	{
		string GetCaptcha(Image image);
	}
}
