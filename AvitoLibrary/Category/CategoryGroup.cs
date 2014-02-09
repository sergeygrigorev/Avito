using System.Collections.Generic;

namespace AvitoLibrary.Category
{
	public class CategoryGroup
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Category> Categories { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
