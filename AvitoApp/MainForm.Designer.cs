namespace AvitoApp
{
	partial class MainForm
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
			this.Textbox = new System.Windows.Forms.TextBox();
			this.Button = new System.Windows.Forms.Button();
			this.Label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Textbox
			// 
			this.Textbox.Location = new System.Drawing.Point(146, 50);
			this.Textbox.Name = "Textbox";
			this.Textbox.Size = new System.Drawing.Size(270, 20);
			this.Textbox.TabIndex = 0;
			// 
			// Button
			// 
			this.Button.Location = new System.Drawing.Point(341, 76);
			this.Button.Name = "Button";
			this.Button.Size = new System.Drawing.Size(75, 23);
			this.Button.TabIndex = 1;
			this.Button.Text = "Map";
			this.Button.UseVisualStyleBackColor = true;
			this.Button.Click += new System.EventHandler(this.Button_Click);
			// 
			// Label
			// 
			this.Label.AutoSize = true;
			this.Label.Location = new System.Drawing.Point(146, 133);
			this.Label.Name = "Label";
			this.Label.Size = new System.Drawing.Size(0, 13);
			this.Label.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(553, 262);
			this.Controls.Add(this.Label);
			this.Controls.Add(this.Button);
			this.Controls.Add(this.Textbox);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox Textbox;
		private System.Windows.Forms.Button Button;
		private System.Windows.Forms.Label Label;
	}
}

