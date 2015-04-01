using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AvitoApp
{
	public delegate void MapCallbackDelegate(string res);

	public partial class MapForm : Form
	{
		private MapCallbackDelegate callback;

		public MapForm(MapCallbackDelegate cb, string search)
		{
			InitializeComponent();
			callback = cb;
			Inet.Navigate("file:///D:/Dev/C%23/Avito/MapHtml/index.html#" + search);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			/*
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(Inet.DocumentText);
			HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='map']");
			string lat = node.GetAttributeValue("data-lat", "");
			string lon = node.GetAttributeValue("data-lon", "");
			string zoom = node.GetAttributeValue("data-zoom", "");
			*/
			//callback(Inet.DocumentTitle);
			if(Ready != null)
			{
				var coords = new CoordinateEventArgs(Inet.DocumentTitle);
				Ready(this, coords);
			}
			base.OnClosing(e);
		}


		public event EventHandler<CoordinateEventArgs> Ready;

	}
}
