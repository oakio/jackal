namespace JackalHost.Monitors
{
    partial class TileControl
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPirates = new System.Windows.Forms.Label();
            this.lblGold = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPirates
            // 
            this.lblPirates.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPirates.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPirates.ForeColor = System.Drawing.Color.White;
            this.lblPirates.Location = new System.Drawing.Point(0, 0);
            this.lblPirates.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPirates.Name = "lblPirates";
            this.lblPirates.Size = new System.Drawing.Size(42, 21);
            this.lblPirates.TabIndex = 0;
            this.lblPirates.Text = "0";
            // 
            // lblGold
            // 
            this.lblGold.BackColor = System.Drawing.Color.Transparent;
            this.lblGold.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblGold.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGold.ForeColor = System.Drawing.Color.White;
            this.lblGold.Location = new System.Drawing.Point(0, 25);
            this.lblGold.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGold.Name = "lblGold";
            this.lblGold.Size = new System.Drawing.Size(42, 21);
            this.lblGold.TabIndex = 1;
            this.lblGold.Text = "0";
            this.lblGold.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblPosition.Font = new System.Drawing.Font("Lucida Console", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPosition.ForeColor = System.Drawing.Color.Gray;
            this.lblPosition.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblPosition.Location = new System.Drawing.Point(0, 38);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(20, 8);
            this.lblPosition.TabIndex = 2;
            this.lblPosition.Text = "0,0";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // TileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.lblGold);
            this.Controls.Add(this.lblPirates);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TileControl";
            this.Size = new System.Drawing.Size(42, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblPirates;
		public System.Windows.Forms.Label lblGold;
        public System.Windows.Forms.Label lblPosition;
    }
}
