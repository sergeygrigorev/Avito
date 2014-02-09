namespace AvitoLibrary.UserInfo
{
	interface IUserService
	{
		User Default(bool reload = false);
	}
}
