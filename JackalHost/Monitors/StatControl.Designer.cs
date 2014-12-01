namespace JackalHost.Monitors
{
	partial class StatControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtBox
			// 
			this.txtBox.BackColor = System.Drawing.Color.Black;
			this.txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtBox.ForeColor = System.Drawing.Color.White;
			this.txtBox.Location = new System.Drawing.Point(0, 0);
			this.txtBox.Multiline = true;
			this.txtBox.Name = "txtBox";
			this.txtBox.Size = new System.Drawing.Size(530, 36);
			this.txtBox.TabIndex = 0;
			this.txtBox.Text = "0";
			// 
			// StatControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtBox);
			this.Name = "StatControl";
			this.Size = new System.Drawing.Size(530, 36);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox txtBox;

	}
}
