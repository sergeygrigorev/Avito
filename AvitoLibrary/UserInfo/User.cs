using System.Text;

namespace AvitoLibrary.UserInfo
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Manager { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool DenyEmails { get; set; }

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();

			s.AppendLine(Name);
			s.AppendLine(Phone);
			s.AppendLine(Email);
			s.Append(DenyEmails);

			return s.ToString();
		}
	}
}
