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
            this.lblGold = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblGold
            // 
            this.lblGold.AutoSize = true;
            this.lblGold.BackColor = System.Drawing.Color.Gold;
            this.lblGold.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGold.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblGold.ForeColor = System.Drawing.Color.Black;
            this.lblGold.Location = new System.Drawing.Point(30, 0);
            this.lblGold.Name = "lblGold";
            this.lblGold.Size = new System.Drawing.Size(26, 29);
            this.lblGold.TabIndex = 1;
            this.lblGold.Text = "0";
            this.lblGold.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblGold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TileControl";
            this.Size = new System.Drawing.Size(56, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblGold;
    }
}
