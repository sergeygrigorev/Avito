namespace AvitoLibrary.Location
{
	public class UserLocation
	{
		public City City { get; set; }
		public Metro Metro { get; set; }
		public District District { get; set; }
		public Road Road { get; set; }

		public override string ToString()
		{
			return City.ToString() + "\n" + Metro.ToString();
		}
	}
}
