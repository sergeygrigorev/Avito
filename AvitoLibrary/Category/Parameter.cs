using System.Collections.Generic;

namespace AvitoLibrary.Category
{
	public class Parameter
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ParameterType Type { get; set; }
		public bool Children { get; set; }
		public string Units { get; set; }
		public List<ParameterValue> Values { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
