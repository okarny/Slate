namespace Slate
{
    partial class Groups
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
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.btnRemoveMember = new System.Windows.Forms.Button();
            this.btnRemoveGroup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddMember = new System.Windows.Forms.Button();
            this.btnEditGroup = new System.Windows.Forms.Button();
            this.dataGridGroups = new System.Windows.Forms.DataGridView();
            this.dataGridGroupMembers = new System.Windows.Forms.DataGridView();
            this.dataGridClients = new System.Windows.Forms.DataGridView();
            this.btnRandomizeGroups = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroupMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridClients)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddGroup.Location = new System.Drawing.Point(13, 168);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(41, 23);
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.Text = "+";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnRemoveMember
            // 
            this.btnRemoveMember.Location = new System.Drawing.Point(696, 168);
            this.btnRemoveMember.Name = "btnRemoveMember";
            this.btnRemoveMember.Size = new System.Drawing.Size(49, 23);
            this.btnRemoveMember.TabIndex = 3;
            this.btnRemoveMember.Text = "↓";
            this.btnRemoveMember.UseVisualStyleBackColor = true;
            this.btnRemoveMember.Click += new System.EventHandler(this.btnRemoveMember_Click);
            // 
            // btnRemoveGroup
            // 
            this.btnRemoveGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveGroup.Location = new System.Drawing.Point(60, 168);
            this.btnRemoveGroup.Name = "btnRemoveGroup";
            this.btnRemoveGroup.Size = new System.Drawing.Size(42, 23);
            this.btnRemoveGroup.TabIndex = 4;
            this.btnRemoveGroup.Text = "-";
            this.btnRemoveGroup.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "GROUPS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(398, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "GROUP MEMBERS";
            // 
            // btnAddMember
            // 
            this.btnAddMember.Location = new System.Drawing.Point(641, 168);
            this.btnAddMember.Name = "btnAddMember";
            this.btnAddMember.Size = new System.Drawing.Size(49, 23);
            this.btnAddMember.TabIndex = 3;
            this.btnAddMember.Text = "↑";
            this.btnAddMember.UseVisualStyleBackColor = true;
            this.btnAddMember.Click += new System.EventHandler(this.btnAddMember_Click);
            // 
            // btnEditGroup
            // 
            this.btnEditGroup.Location = new System.Drawing.Point(281, 168);
            this.btnEditGroup.Name = "btnEditGroup";
            this.btnEditGroup.Size = new System.Drawing.Size(75, 23);
            this.btnEditGroup.TabIndex = 6;
            this.btnEditGroup.Text = "Edit Group";
            this.btnEditGroup.UseVisualStyleBackColor = true;
            // 
            // dataGridGroups
            // 
            this.dataGridGroups.AllowUserToAddRows = false;
            this.dataGridGroups.AllowUserToDeleteRows = false;
            this.dataGridGroups.AllowUserToResizeColumns = false;
            this.dataGridGroups.AllowUserToResizeRows = false;
            this.dataGridGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridGroups.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridGroups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGroups.Location = new System.Drawing.Point(12, 29);
            this.dataGridGroups.Name = "dataGridGroups";
            this.dataGridGroups.RowHeadersVisible = false;
            this.dataGridGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridGroups.Size = new System.Drawing.Size(344, 133);
            this.dataGridGroups.TabIndex = 7;
            this.dataGridGroups.SelectionChanged += new System.EventHandler(this.dataGridGroups_SelectionChanged);
            // 
            // dataGridGroupMembers
            // 
            this.dataGridGroupMembers.AllowUserToAddRows = false;
            this.dataGridGroupMembers.AllowUserToDeleteRows = false;
            this.dataGridGroupMembers.AllowUserToResizeColumns = false;
            this.dataGridGroupMembers.AllowUserToResizeRows = false;
            this.dataGridGroupMembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridGroupMembers.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridGroupMembers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridGroupMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridGroupMembers.Location = new System.Drawing.Point(401, 26);
            this.dataGridGroupMembers.Name = "dataGridGroupMembers";
            this.dataGridGroupMembers.RowHeadersVisible = false;
            this.dataGridGroupMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridGroupMembers.Size = new System.Drawing.Size(344, 133);
            this.dataGridGroupMembers.TabIndex = 8;
            this.dataGridGroupMembers.SelectionChanged += new System.EventHandler(this.dataGridGroupMembers_SelectionChanged);
            // 
            // dataGridClients
            // 
            this.dataGridClients.AllowUserToAddRows = false;
            this.dataGridClients.AllowUserToDeleteRows = false;
            this.dataGridClients.AllowUserToResizeColumns = false;
            this.dataGridClients.AllowUserToResizeRows = false;
            this.dataGridClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridClients.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridClients.Location = new System.Drawing.Point(12, 208);
            this.dataGridClients.Name = "dataGridClients";
            this.dataGridClients.RowHeadersVisible = false;
            this.dataGridClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridClients.Size = new System.Drawing.Size(733, 151);
            this.dataGridClients.TabIndex = 9;
            this.dataGridClients.SelectionChanged += new System.EventHandler(this.dataGridClients_SelectionChanged);
            // 
            // btnRandomizeGroups
            // 
            this.btnRandomizeGroups.Location = new System.Drawing.Point(401, 168);
            this.btnRandomizeGroups.Name = "btnRandomizeGroups";
            this.btnRandomizeGroups.Size = new System.Drawing.Size(158, 23);
            this.btnRandomizeGroups.TabIndex = 10;
            this.btnRandomizeGroups.Text = "Randomize Groups...";
            this.btnRandomizeGroups.UseVisualStyleBackColor = true;
            this.btnRandomizeGroups.Click += new System.EventHandler(this.btnRandomizeGroups_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(670, 366);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(13, 366);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(89, 23);
            this.btnActivate.TabIndex = 11;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // Groups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 401);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRandomizeGroups);
            this.Controls.Add(this.dataGridClients);
            this.Controls.Add(this.dataGridGroupMembers);
            this.Controls.Add(this.dataGridGroups);
            this.Controls.Add(this.btnEditGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemoveGroup);
            this.Controls.Add(this.btnAddMember);
            this.Controls.Add(this.btnRemoveMember);
            this.Controls.Add(this.btnAddGroup);
            this.Name = "Groups";
            this.Text = "Groups";
            this.Load += new System.EventHandler(this.Groups_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridGroupMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.Button btnRemoveMember;
        private System.Windows.Forms.Button btnRemoveGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddMember;
        private System.Windows.Forms.Button btnEditGroup;
        private System.Windows.Forms.DataGridView dataGridGroups;
        private System.Windows.Forms.DataGridView dataGridGroupMembers;
        private System.Windows.Forms.DataGridView dataGridClients;
        private System.Windows.Forms.Button btnRandomizeGroups;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnActivate;
    }
}