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
			MapForm map = new MapForm(Callback, Textbox.Text);
			map.ShowDialog();
		}

		private void Callback(string res)
		{
			Label.Text = res;
		}
	}
}
