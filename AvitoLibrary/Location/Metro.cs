namespace AvitoLibrary.Location
{
	public class Metro
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
