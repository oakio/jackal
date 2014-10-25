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
            this.components = new System.ComponentModel.Container();
            this.gameSplitContainer = new System.Windows.Forms.SplitContainer();
            this.statSplitContainer = new System.Windows.Forms.SplitContainer();
            this.oneTurnBtn = new System.Windows.Forms.Button();
            this.pauseGameBtn = new System.Windows.Forms.Button();
            this.newGameBtn = new System.Windows.Forms.Button();
            this.gameTurnTimer = new System.Windows.Forms.Timer(this.components);
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
            this.gameSplitContainer.Size = new System.Drawing.Size(1261, 722);
            this.gameSplitContainer.SplitterDistance = 725;
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
            this.statSplitContainer.Panel2.Controls.Add(this.oneTurnBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.pauseGameBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.newGameBtn);
            this.statSplitContainer.Size = new System.Drawing.Size(532, 722);
            this.statSplitContainer.SplitterDistance = 325;
            this.statSplitContainer.TabIndex = 1;
            // 
            // oneTurnBtn
            // 
            this.oneTurnBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.oneTurnBtn.Location = new System.Drawing.Point(4, 59);
            this.oneTurnBtn.Margin = new System.Windows.Forms.Padding(4);
            this.oneTurnBtn.Name = "oneTurnBtn";
            this.oneTurnBtn.Size = new System.Drawing.Size(249, 47);
            this.oneTurnBtn.TabIndex = 2;
            this.oneTurnBtn.Text = "One turn";
            this.oneTurnBtn.UseVisualStyleBackColor = true;
            this.oneTurnBtn.Click += new System.EventHandler(this.oneTurnBtn_Click);
            // 
            // pauseGameBtn
            // 
            this.pauseGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pauseGameBtn.Location = new System.Drawing.Point(3, 2);
            this.pauseGameBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pauseGameBtn.Name = "pauseGameBtn";
            this.pauseGameBtn.Size = new System.Drawing.Size(249, 50);
            this.pauseGameBtn.TabIndex = 1;
            this.pauseGameBtn.Text = "Pause game";
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
            // gameTurnTimer
            // 
            this.gameTurnTimer.Interval = 10;
            this.gameTurnTimer.Tick += new System.EventHandler(this.gameTurnTimer_Tick);
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 722);
            this.Controls.Add(this.gameSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MonitorForm";
            this.Text = "MonitorForm";
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
        private System.Windows.Forms.Timer gameTurnTimer;
        private System.Windows.Forms.SplitContainer statSplitContainer;
        private System.Windows.Forms.Button newGameBtn;
        private System.Windows.Forms.Button pauseGameBtn;
		private System.Windows.Forms.Button oneTurnBtn;

	}
}