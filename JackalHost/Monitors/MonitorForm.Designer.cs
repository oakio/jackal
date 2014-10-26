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
            this.txtTurn = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pauseGameBtn = new System.Windows.Forms.Button();
            this.newGameBtn = new System.Windows.Forms.Button();
            this.gameTurnTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gameSplitContainer)).BeginInit();
            this.gameSplitContainer.Panel2.SuspendLayout();
            this.gameSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statSplitContainer)).BeginInit();
            this.statSplitContainer.Panel1.SuspendLayout();
            this.statSplitContainer.Panel2.SuspendLayout();
            this.statSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameSplitContainer
            // 
            this.gameSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.gameSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.gameSplitContainer.Name = "gameSplitContainer";
            // 
            // gameSplitContainer.Panel2
            // 
            this.gameSplitContainer.Panel2.Controls.Add(this.statSplitContainer);
            this.gameSplitContainer.Size = new System.Drawing.Size(1142, 708);
            this.gameSplitContainer.SplitterDistance = 656;
            this.gameSplitContainer.SplitterWidth = 3;
            this.gameSplitContainer.TabIndex = 0;
            // 
            // statSplitContainer
            // 
            this.statSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.statSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.statSplitContainer.Name = "statSplitContainer";
            this.statSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // statSplitContainer.Panel1
            // 
            this.statSplitContainer.Panel1.Controls.Add(this.txtTurn);
            // 
            // statSplitContainer.Panel2
            // 
            this.statSplitContainer.Panel2.Controls.Add(this.button2);
            this.statSplitContainer.Panel2.Controls.Add(this.button1);
            this.statSplitContainer.Panel2.Controls.Add(this.pauseGameBtn);
            this.statSplitContainer.Panel2.Controls.Add(this.newGameBtn);
            this.statSplitContainer.Size = new System.Drawing.Size(483, 708);
            this.statSplitContainer.SplitterDistance = 319;
            this.statSplitContainer.SplitterWidth = 3;
            this.statSplitContainer.TabIndex = 1;
            // 
            // txtTurn
            // 
            this.txtTurn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTurn.Location = new System.Drawing.Point(0, 0);
            this.txtTurn.Margin = new System.Windows.Forms.Padding(2);
            this.txtTurn.Name = "txtTurn";
            this.txtTurn.Size = new System.Drawing.Size(481, 30);
            this.txtTurn.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.button2.Location = new System.Drawing.Point(131, 48);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 38);
            this.button2.TabIndex = 4;
            this.button2.Text = "+4";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.button1.Location = new System.Drawing.Point(3, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "One turn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pauseGameBtn
            // 
            this.pauseGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pauseGameBtn.Location = new System.Drawing.Point(2, 2);
            this.pauseGameBtn.Margin = new System.Windows.Forms.Padding(2);
            this.pauseGameBtn.Name = "pauseGameBtn";
            this.pauseGameBtn.Size = new System.Drawing.Size(187, 41);
            this.pauseGameBtn.TabIndex = 1;
            this.pauseGameBtn.Text = "Pause game";
            this.pauseGameBtn.UseVisualStyleBackColor = true;
            this.pauseGameBtn.Click += new System.EventHandler(this.pauseGameBtn_Click);
            // 
            // newGameBtn
            // 
            this.newGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newGameBtn.Location = new System.Drawing.Point(3, 91);
            this.newGameBtn.Margin = new System.Windows.Forms.Padding(2);
            this.newGameBtn.Name = "newGameBtn";
            this.newGameBtn.Size = new System.Drawing.Size(187, 41);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 708);
            this.Controls.Add(this.gameSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MonitorForm";
            this.Text = "MonitorForm";
            this.Load += new System.EventHandler(this.MonitorForm_Load);
            this.gameSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gameSplitContainer)).EndInit();
            this.gameSplitContainer.ResumeLayout(false);
            this.statSplitContainer.Panel1.ResumeLayout(false);
            this.statSplitContainer.Panel1.PerformLayout();
            this.statSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statSplitContainer)).EndInit();
            this.statSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.SplitContainer gameSplitContainer;
		public System.Windows.Forms.TextBox txtTurn;
        private System.Windows.Forms.Timer gameTurnTimer;
        private System.Windows.Forms.SplitContainer statSplitContainer;
        private System.Windows.Forms.Button newGameBtn;
        private System.Windows.Forms.Button pauseGameBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

	}
}