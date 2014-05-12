namespace Slate
{
    partial class Welcome
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Welcome));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnResumeSession = new System.Windows.Forms.Button();
            this.btnNewBoardSession = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Previous Sessions";
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(291, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(160, 160);
            this.panel1.TabIndex = 2;
            // 
            // btnResumeSession
            // 
            this.btnResumeSession.Enabled = false;
            this.btnResumeSession.Location = new System.Drawing.Point(12, 259);
            this.btnResumeSession.Name = "btnResumeSession";
            this.btnResumeSession.Size = new System.Drawing.Size(119, 23);
            this.btnResumeSession.TabIndex = 3;
            this.btnResumeSession.Text = "Resume Session";
            this.btnResumeSession.UseVisualStyleBackColor = true;
            this.btnResumeSession.Click += new System.EventHandler(this.btnResumeSession_Click);
            // 
            // btnNewBoardSession
            // 
            this.btnNewBoardSession.Location = new System.Drawing.Point(369, 259);
            this.btnNewBoardSession.Name = "btnNewBoardSession";
            this.btnNewBoardSession.Size = new System.Drawing.Size(127, 23);
            this.btnNewBoardSession.TabIndex = 3;
            this.btnNewBoardSession.Text = "New Board Session";
            this.btnNewBoardSession.UseVisualStyleBackColor = true;
            this.btnNewBoardSession.Click += new System.EventHandler(this.btnNewBoardSession_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point(285, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 33);
            this.label2.TabIndex = 4;
            this.label2.Text = "Slate";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataView
            // 
            this.dataView.AllowUserToAddRows = false;
            this.dataView.AllowUserToDeleteRows = false;
            this.dataView.AllowUserToResizeColumns = false;
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Location = new System.Drawing.Point(15, 28);
            this.dataView.Name = "dataView";
            this.dataView.ReadOnly = true;
            this.dataView.RowHeadersVisible = false;
            this.dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataView.Size = new System.Drawing.Size(264, 225);
            this.dataView.TabIndex = 5;
            this.dataView.SelectionChanged += new System.EventHandler(this.dataView_SelectionChanged);
            this.dataView.DoubleClick += new System.EventHandler(this.dataView_DoubleClick);
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 294);
            this.Controls.Add(this.dataView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnNewBoardSession);
            this.Controls.Add(this.btnResumeSession);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "Welcome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome to Slate";
            this.Load += new System.EventHandler(this.Welcome_Load);
            this.VisibleChanged += new System.EventHandler(this.Welcome_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnResumeSession;
        private System.Windows.Forms.Button btnNewBoardSession;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataView;
    }
}