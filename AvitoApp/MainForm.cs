using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvitoApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, EventArgs e)
		{
			//MapForm map = new MapForm(Callback, Textbox.Text);
			//map.ShowDialog();

			//var result = MessageBox.Show("DDDDDD", "DDDDDDD");

			//IMapComponent map = (IMapComponent) new MapForm(null, null);
			//Label.Text = map.Select().ToString();

			MapForm map = new MapForm(Callback, Textbox.Text);

			map.Ready += map_Ready;
			map.ShowDialog();





		}

		void map_Ready(object sender, CoordinateEventArgs e)
		{
			Label.Text = "Selected";
		}

		private void Callback(string res)
		{
			Label.Text = res;
		}
	}

	public class CoordinateEventArgs : EventArgs
	{
		public double Lat;
		public double Lon;
		public int Zoom { get; set; }

		public CoordinateEventArgs(string s)
		{
			
		}

		public override string ToString()
		{
			return String.Format("({0}, {1}), {2}", Lat, Lon, Zoom);
		}
	}

	public interface IMapComponent
	{
		CoordinateEventArgs Select();
	}

}
