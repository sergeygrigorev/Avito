using System.Collections.Generic;

namespace AvitoLibrary.Category
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Parameter> Parameters { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
