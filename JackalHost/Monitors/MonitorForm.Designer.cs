namespace JackalHost.Monitors
{
	partial class MonitorForm
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
            this.gameSplitContainer = new System.Windows.Forms.SplitContainer();
            this.statSplitContainer = new System.Windows.Forms.SplitContainer();
            this.nextTurnesBtn = new System.Windows.Forms.Button();
            this.nextOneBtn = new System.Windows.Forms.Button();
            this.slowTurnesBtn = new System.Windows.Forms.Button();
            this.fastTurnesBtn = new System.Windows.Forms.Button();
            this.pauseGameBtn = new System.Windows.Forms.Button();
            this.newGameBtn = new System.Windows.Forms.Button();
            this.prevOneBtn = new System.Windows.Forms.Button();
            this.prevTurnesBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gameSplitContainer)).BeginInit();
            this.gameSplitContainer.Panel2.SuspendLayout();
            this.gameSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statSplitContainer)).BeginInit();
            this.statSplitContainer.Panel2.SuspendLayout();
            this.statSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameSplitContainer
            // 
            this.gameSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.gameSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gameSplitContainer.Name = "gameSplitContainer";
            // 
            // gameSplitContainer.Panel1
            // 
            this.gameSplitContainer.Panel1.Resize += new System.EventHandler(this.gameSplitContainer_Panel1_Resize);
            // 
            // gameSplitContainer.Panel2
            // 
            this.gameSplitContainer.Panel2.Controls.Add(this.statSplitContainer);
            this.gameSplitContainer.Size = new System.Drawing.Size(1261, 894);
            this.gameSplitContainer.SplitterDistance = 986;
            this.gameSplitContainer.TabIndex = 0;
            // 
            // statSplitContainer
            // 
            this.statSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.statSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.statSplitContainer.Name = "statSplitContainer";
            this.statSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // statSplitContainer.Panel1
            // 
            this.statSplitContainer.Panel1.Resize += new System.EventHandler(this.statSplitContainer_Panel1_Resize);
            // 
            // statSplitContainer.Panel2
            // 
            this.statSplitContainer.Panel2.Controls.Add(this.prevTurnesBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.prevOneBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.nextTurnesBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.nextOneBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.slowTurnesBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.fastTurnesBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.pauseGameBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.newGameBtn);
            this.statSplitContainer.Size = new System.Drawing.Size(271, 894);
            this.statSplitContainer.SplitterDistance = 394;
            this.statSplitContainer.TabIndex = 1;
            // 
            // nextTurnesBtn
            // 
            this.nextTurnesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.nextTurnesBtn.Location = new System.Drawing.Point(192, 168);
            this.nextTurnesBtn.Margin = new System.Windows.Forms.Padding(4);
            this.nextTurnesBtn.Name = "nextTurnesBtn";
            this.nextTurnesBtn.Size = new System.Drawing.Size(60, 47);
            this.nextTurnesBtn.TabIndex = 7;
            this.nextTurnesBtn.Text = "+4";
            this.nextTurnesBtn.UseVisualStyleBackColor = true;
            this.nextTurnesBtn.Click += new System.EventHandler(this.nextTurnesBtn_Click);
            // 
            // nextOneBtn
            // 
            this.nextOneBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.nextOneBtn.Location = new System.Drawing.Point(132, 168);
            this.nextOneBtn.Margin = new System.Windows.Forms.Padding(4);
            this.nextOneBtn.Name = "nextOneBtn";
            this.nextOneBtn.Size = new System.Drawing.Size(60, 47);
            this.nextOneBtn.TabIndex = 6;
            this.nextOneBtn.Text = "+1";
            this.nextOneBtn.UseVisualStyleBackColor = true;
            this.nextOneBtn.Click += new System.EventHandler(this.nextOneBtn_Click);
            // 
            // slowTurnesBtn
            // 
            this.slowTurnesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.slowTurnesBtn.Location = new System.Drawing.Point(4, 58);
            this.slowTurnesBtn.Margin = new System.Windows.Forms.Padding(4);
            this.slowTurnesBtn.Name = "slowTurnesBtn";
            this.slowTurnesBtn.Size = new System.Drawing.Size(120, 47);
            this.slowTurnesBtn.TabIndex = 3;
            this.slowTurnesBtn.Text = "Slower";
            this.slowTurnesBtn.UseVisualStyleBackColor = true;
            this.slowTurnesBtn.Click += new System.EventHandler(this.slowTurnesBtn_Click);
            // 
            // fastTurnesBtn
            // 
            this.fastTurnesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.fastTurnesBtn.Location = new System.Drawing.Point(132, 58);
            this.fastTurnesBtn.Margin = new System.Windows.Forms.Padding(4);
            this.fastTurnesBtn.Name = "fastTurnesBtn";
            this.fastTurnesBtn.Size = new System.Drawing.Size(120, 47);
            this.fastTurnesBtn.TabIndex = 2;
            this.fastTurnesBtn.Text = "Faster";
            this.fastTurnesBtn.UseVisualStyleBackColor = true;
            this.fastTurnesBtn.Click += new System.EventHandler(this.fastTurnesBtn_Click);
            // 
            // pauseGameBtn
            // 
            this.pauseGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pauseGameBtn.Location = new System.Drawing.Point(3, 2);
            this.pauseGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pauseGameBtn.Name = "pauseGameBtn";
            this.pauseGameBtn.Size = new System.Drawing.Size(249, 50);
            this.pauseGameBtn.TabIndex = 1;
            this.pauseGameBtn.Text = "Start game";
            this.pauseGameBtn.UseVisualStyleBackColor = true;
            this.pauseGameBtn.Click += new System.EventHandler(this.pauseGameBtn_Click);
            // 
            // newGameBtn
            // 
            this.newGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newGameBtn.Location = new System.Drawing.Point(4, 112);
            this.newGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newGameBtn.Name = "newGameBtn";
            this.newGameBtn.Size = new System.Drawing.Size(249, 50);
            this.newGameBtn.TabIndex = 0;
            this.newGameBtn.Text = "New game";
            this.newGameBtn.UseVisualStyleBackColor = true;
            this.newGameBtn.Click += new System.EventHandler(this.newGameBtn_Click);
            // 
            // prevOneBtn
            // 
            this.prevOneBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.prevOneBtn.Location = new System.Drawing.Point(64, 168);
            this.prevOneBtn.Margin = new System.Windows.Forms.Padding(4);
            this.prevOneBtn.Name = "prevOneBtn";
            this.prevOneBtn.Size = new System.Drawing.Size(60, 47);
            this.prevOneBtn.TabIndex = 8;
            this.prevOneBtn.Text = "-1";
            this.prevOneBtn.UseVisualStyleBackColor = true;
            this.prevOneBtn.Click += new System.EventHandler(this.prevOneBtn_Click);
            // 
            // prevTurnesBtn
            // 
            this.prevTurnesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.prevTurnesBtn.Location = new System.Drawing.Point(4, 168);
            this.prevTurnesBtn.Margin = new System.Windows.Forms.Padding(4);
            this.prevTurnesBtn.Name = "prevTurnesBtn";
            this.prevTurnesBtn.Size = new System.Drawing.Size(60, 47);
            this.prevTurnesBtn.TabIndex = 9;
            this.prevTurnesBtn.Text = "-4";
            this.prevTurnesBtn.UseVisualStyleBackColor = true;
            this.prevTurnesBtn.Click += new System.EventHandler(this.prevTurnesBtn_Click);
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 894);
            this.Controls.Add(this.gameSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MonitorForm";
            this.Text = "Jackal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MonitorForm_FormClosed);
            this.Load += new System.EventHandler(this.MonitorForm_Load);
            this.gameSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameSplitContainer)).EndInit();
            this.gameSplitContainer.ResumeLayout(false);
            this.statSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statSplitContainer)).EndInit();
            this.statSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        public System.Windows.Forms.SplitContainer gameSplitContainer;
        private System.Windows.Forms.SplitContainer statSplitContainer;
        private System.Windows.Forms.Button newGameBtn;
        private System.Windows.Forms.Button pauseGameBtn;
		private System.Windows.Forms.Button fastTurnesBtn;
        private System.Windows.Forms.Button slowTurnesBtn;
        private System.Windows.Forms.Button nextTurnesBtn;
        private System.Windows.Forms.Button nextOneBtn;
        private System.Windows.Forms.Button prevTurnesBtn;
        private System.Windows.Forms.Button prevOneBtn;

	}
}