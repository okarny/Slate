namespace Slate
{
    partial class Preferences
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseRoster = new System.Windows.Forms.Button();
            this.txtSelectedRoster = new System.Windows.Forms.TextBox();
            this.chckSelectRoster = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(447, 234);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnBrowseRoster);
            this.tabPage1.Controls.Add(this.txtSelectedRoster);
            this.tabPage1.Controls.Add(this.chckSelectRoster);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(439, 208);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ROSTER SETTINGS";
            // 
            // btnBrowseRoster
            // 
            this.btnBrowseRoster.Location = new System.Drawing.Point(393, 56);
            this.btnBrowseRoster.Name = "btnBrowseRoster";
            this.btnBrowseRoster.Size = new System.Drawing.Size(40, 23);
            this.btnBrowseRoster.TabIndex = 2;
            this.btnBrowseRoster.Text = "...";
            this.btnBrowseRoster.UseVisualStyleBackColor = true;
            // 
            // txtSelectedRoster
            // 
            this.txtSelectedRoster.BackColor = System.Drawing.SystemColors.Menu;
            this.txtSelectedRoster.Location = new System.Drawing.Point(6, 56);
            this.txtSelectedRoster.Name = "txtSelectedRoster";
            this.txtSelectedRoster.Size = new System.Drawing.Size(381, 20);
            this.txtSelectedRoster.TabIndex = 1;
            // 
            // chckSelectRoster
            // 
            this.chckSelectRoster.AutoSize = true;
            this.chckSelectRoster.Location = new System.Drawing.Point(6, 32);
            this.chckSelectRoster.Name = "chckSelectRoster";
            this.chckSelectRoster.Size = new System.Drawing.Size(179, 17);
            this.chckSelectRoster.TabIndex = 0;
            this.chckSelectRoster.Text = "Use selected roster upon startup";
            this.chckSelectRoster.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(439, 208);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Other";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 258);
            this.Controls.Add(this.tabControl1);
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseRoster;
        private System.Windows.Forms.TextBox txtSelectedRoster;
        private System.Windows.Forms.CheckBox chckSelectRoster;
        private System.Windows.Forms.TabPage tabPage2;
    }
}