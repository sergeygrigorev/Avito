using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace AvitoLibrary.Category
{
	class CategoryService
	{
		public List<CategoryGroup> GetCategories()
		{
			string data = GlobalParseService.NewAdHtml;
			string json = GlobalParseService.CatJson;
			return ParseJson(json, data);
		}

		private List<CategoryGroup> ParseJson(string jsonRawData, string html)
		{
			JObject json = JObject.Parse(jsonRawData);

			List<CategoryGroup> list = new List<CategoryGroup>();
			List<Category> lc = new List<Category>();

			foreach (KeyValuePair<string, JToken> k1 in json)
			{
				if (k1.Value.Type == JTokenType.Array)
				{
					list.Add(new CategoryGroup { Id = Int32.Parse(k1.Key), Name = ((JArray)k1.Value)[0].ToString() });
				}
				else
				{
					lc.Add(CreateCategory(Int32.Parse(k1.Key), (JObject)k1.Value));
				}
			}

			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNode root = doc.GetElementbyId("fld_category_id");

			foreach (HtmlNode x in root.ChildNodes)
			{
				if (x.Name != "optgroup")
					continue;
				string label = x.GetAttributeValue("label", "");
				CategoryGroup group = list.First(p => p.Name == label);
				group.Categories = new List<Category>();
				foreach (HtmlNode y in x.ChildNodes)
				{
					if (y.Name != "option")
						continue;
					int catId = y.GetAttributeValue("value", -1);
					Category c;
					try
					{
						c = lc.First(p => p.Id == catId);
					}
					catch (Exception e)
					{
						c = new Category();
						c.Id = catId;
						c.Name = y.NextSibling.InnerText;
					}
					group.Categories.Add(c);
				}
			}

			list = list.Where(p => p.Categories != null).ToList();

			return list;
		}

		private Category CreateCategory(int id, JObject json)
		{
			Category c = new Category();
			c.Id = id;
			c.Name = json["0"].ToString();
			c.Parameters = new List<Parameter>();
			foreach (KeyValuePair<string, JToken> x in json)
				if (x.Key[0] == '_')
					c.Parameters.Add(CreateParameter(Int32.Parse(x.Key.Substring(1)), toJObject(x.Value)));

			return c;
		}

		private JObject toJObject(JToken json)
		{
			if (json.Type == JTokenType.Object)
				return (JObject)json;
			JObject j = new JObject();
			JArray arr = (JArray)json;
			for (int i = 0; i < arr.Count; i++)
				if (arr[i] != null)
					j.Add(i.ToString(), arr[i]);

			return j;
		}

		private Parameter CreateParameter(int id, JObject json)
		{
			Parameter p = new Parameter();
			p.Values = new List<ParameterValue>();
			p.Id = id;
			p.Name = json["0"]["name"].ToString();
			p.Children = (json["0"]["children"] != null);
			p.Units = json["0"]["units"].ToString();
			string t = json["0"]["type"].ToString();
			/*if(t == "s")
				p.Type = ParameterType.Select;
			else if(t=="c")
				p.Type = ParameterType.Text;*/
			switch (t)
			{
				case "s": p.Type = ParameterType.Select; break;
				case "c": p.Type = ParameterType.Text; break;
				case "i": p.Type = ParameterType.IntegerM; break;
				case "n": p.Type = ParameterType.IntegerS; break;
				default: p.Type = ParameterType.Select; break;
			}
			foreach (KeyValuePair<string, JToken> x in json)
			{
				if (x.Key[0] != '_')
					continue;
				ParameterValue pv = new ParameterValue();
				pv.Id = Int32.Parse(x.Key.Substring(1));
				if (x.Value.Type == JTokenType.Object)
				{
					pv.Name = x.Value["0"].ToString();
					pv.SubParameters = new List<Parameter>();
					foreach (KeyValuePair<string, JToken> y in (JObject)x.Value)
						if (y.Key[0] == '_')
							pv.SubParameters.Add(CreateParameter(Int32.Parse(y.Key.Substring(1)), toJObject(y.Value)));
				}
				else pv.Name = x.Value[0].ToString();
				p.Values.Add(pv);
			}

			return p;
		}
	}
}
