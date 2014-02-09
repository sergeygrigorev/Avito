using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvitoLibrary.Category;

namespace AvitoLibrary.Advert
{
	public class Advertisement
	{
		public int Id { get; set; }
		public UserInfo.User User { get; set; }
		public Location.UserLocation Location { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public int CategoryId { get; set; }
		public Dictionary<int, int> Parameters { get; set; }
		public int Price { get; set; }
		public List<Image> Images { get; set; }
		public string Url { get; set; }

		public override string ToString()
		{
			CategoryService cats = new CategoryService();
			List<CategoryGroup> catg = cats.GetCategories();
			Category.Category cat = catg.Where(p => p.Categories.Count(q => q.Id == CategoryId) > 0).Select(p=>p.Categories.Where(q=>q.Id==CategoryId).First()).First();
			StringBuilder s = new StringBuilder();

			s.Append(User);
			s.AppendLine();
			s.Append(Location);
			s.AppendLine();
			s.AppendLine(cat.Name);
			s.Append(Parameters);
			s.AppendLine(Title);
			s.AppendLine(Body);
			s.Append(Price);

			return s.ToString();
		}
	}
}
