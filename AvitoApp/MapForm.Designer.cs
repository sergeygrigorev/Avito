namespace AvitoApp
{
	partial class MapForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Inet = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// Inet
			// 
			this.Inet.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Inet.Location = new System.Drawing.Point(0, 0);
			this.Inet.MinimumSize = new System.Drawing.Size(20, 20);
			this.Inet.Name = "Inet";
			this.Inet.Size = new System.Drawing.Size(680, 410);
			this.Inet.TabIndex = 0;
			// 
			// MapForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(680, 410);
			this.Controls.Add(this.Inet);
			this.Name = "MapForm";
			this.Text = "MapForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser Inet;
	}
}