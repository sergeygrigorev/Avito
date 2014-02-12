namespace AvitoLibrary.UserInfo
{
	interface IUserService
	{
		User GetDefault(bool reload = false);
	}
}
