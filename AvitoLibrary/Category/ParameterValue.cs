using System.Collections.Generic;

namespace AvitoLibrary.Category
{
	public class ParameterValue
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Parameter> SubParameters { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
