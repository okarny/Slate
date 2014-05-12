namespace Slate
{
    partial class frmMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boardSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quizToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pollingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNewClass = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.importRollsheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenColorBlack = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenColorRed = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenColorGreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenColorBlue = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbPenWidth = new System.Windows.Forms.ToolStripComboBox();
            this.eraserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startQuizToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.groupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activateGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gradesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuOperationsRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addonMarketplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlCanvas = new Slate.Canvas();
            this.contextCanvas = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.penToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eraserToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextPanelInsertTextbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.navigateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextCanvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuPen,
            this.eraserToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(876, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator4,
            this.importRollsheetToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boardSessionToolStripMenuItem,
            this.quizToolStripMenuItem,
            this.pollingToolStripMenuItem,
            this.toolStripSeparator3,
            this.mnuNewClass});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // boardSessionToolStripMenuItem
            // 
            this.boardSessionToolStripMenuItem.Name = "boardSessionToolStripMenuItem";
            this.boardSessionToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.boardSessionToolStripMenuItem.Text = "Board Session";
            this.boardSessionToolStripMenuItem.Click += new System.EventHandler(this.boardSessionToolStripMenuItem_Click);
            // 
            // quizToolStripMenuItem
            // 
            this.quizToolStripMenuItem.Name = "quizToolStripMenuItem";
            this.quizToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.quizToolStripMenuItem.Text = "Quiz";
            // 
            // pollingToolStripMenuItem
            // 
            this.pollingToolStripMenuItem.Name = "pollingToolStripMenuItem";
            this.pollingToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.pollingToolStripMenuItem.Text = "Polling";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(144, 6);
            // 
            // mnuNewClass
            // 
            this.mnuNewClass.Name = "mnuNewClass";
            this.mnuNewClass.Size = new System.Drawing.Size(147, 22);
            this.mnuNewClass.Text = "Class...";
            this.mnuNewClass.Click += new System.EventHandler(this.mnuNewClass_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openToolStripMenuItem.Text = "Open Session...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.saveToolStripMenuItem.Text = "Save Session";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(152, 6);
            // 
            // importRollsheetToolStripMenuItem
            // 
            this.importRollsheetToolStripMenuItem.Name = "importRollsheetToolStripMenuItem";
            this.importRollsheetToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.importRollsheetToolStripMenuItem.Text = "Import Roster...";
            // 
            // mnuPen
            // 
            this.mnuPen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPenColor,
            this.cmbPenWidth});
            this.mnuPen.Name = "mnuPen";
            this.mnuPen.Size = new System.Drawing.Size(39, 20);
            this.mnuPen.Text = "Pen";
            this.mnuPen.TextChanged += new System.EventHandler(this.mnuPenWidthSelect);
            // 
            // mnuPenColor
            // 
            this.mnuPenColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPenColorBlack,
            this.mnuPenColorRed,
            this.mnuPenColorGreen,
            this.mnuPenColorBlue,
            this.highlightToolStripMenuItem});
            this.mnuPenColor.Name = "mnuPenColor";
            this.mnuPenColor.Size = new System.Drawing.Size(181, 22);
            this.mnuPenColor.Text = "Color";
            // 
            // mnuPenColorBlack
            // 
            this.mnuPenColorBlack.Name = "mnuPenColorBlack";
            this.mnuPenColorBlack.Size = new System.Drawing.Size(124, 22);
            this.mnuPenColorBlack.Text = "Black";
            this.mnuPenColorBlack.Click += new System.EventHandler(this.mnuPenColorBlack_Click);
            // 
            // mnuPenColorRed
            // 
            this.mnuPenColorRed.Name = "mnuPenColorRed";
            this.mnuPenColorRed.Size = new System.Drawing.Size(124, 22);
            this.mnuPenColorRed.Text = "Red";
            this.mnuPenColorRed.Click += new System.EventHandler(this.mnuPenColorBlack_Click);
            // 
            // mnuPenColorGreen
            // 
            this.mnuPenColorGreen.Name = "mnuPenColorGreen";
            this.mnuPenColorGreen.Size = new System.Drawing.Size(124, 22);
            this.mnuPenColorGreen.Text = "Green";
            this.mnuPenColorGreen.Click += new System.EventHandler(this.mnuPenColorBlack_Click);
            // 
            // mnuPenColorBlue
            // 
            this.mnuPenColorBlue.Name = "mnuPenColorBlue";
            this.mnuPenColorBlue.Size = new System.Drawing.Size(124, 22);
            this.mnuPenColorBlue.Text = "Blue";
            this.mnuPenColorBlue.Click += new System.EventHandler(this.mnuPenColorBlack_Click);
            // 
            // highlightToolStripMenuItem
            // 
            this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
            this.highlightToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.highlightToolStripMenuItem.Text = "Highlight";
            this.highlightToolStripMenuItem.Click += new System.EventHandler(this.mnuPenColorBlack_Click);
            // 
            // cmbPenWidth
            // 
            this.cmbPenWidth.Items.AddRange(new object[] {
            "1",
            "2",
            "4"});
            this.cmbPenWidth.Name = "cmbPenWidth";
            this.cmbPenWidth.Size = new System.Drawing.Size(121, 23);
            this.cmbPenWidth.Text = "Width";
            this.cmbPenWidth.SelectedIndexChanged += new System.EventHandler(this.mnuPenWidthSelect);
            // 
            // eraserToolStripMenuItem
            // 
            this.eraserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startQuizToolStripMenuItem,
            this.pollToolStripMenuItem,
            this.toolStripSeparator5,
            this.groupsToolStripMenuItem,
            this.toolStripSeparator2,
            this.gradesToolStripMenuItem,
            this.toolStripSeparator1,
            this.mnuOperationsRegistration,
            this.connectionsToolStripMenuItem});
            this.eraserToolStripMenuItem.Name = "eraserToolStripMenuItem";
            this.eraserToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.eraserToolStripMenuItem.Text = "Operations";
            // 
            // startQuizToolStripMenuItem
            // 
            this.startQuizToolStripMenuItem.Name = "startQuizToolStripMenuItem";
            this.startQuizToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.startQuizToolStripMenuItem.Text = "Quiz...";
            // 
            // pollToolStripMenuItem
            // 
            this.pollToolStripMenuItem.Name = "pollToolStripMenuItem";
            this.pollToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.pollToolStripMenuItem.Text = "Poll...";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(147, 6);
            // 
            // groupsToolStripMenuItem
            // 
            this.groupsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activateGroupsToolStripMenuItem,
            this.monitorToolStripMenuItem,
            this.toolStripSeparator6,
            this.configureToolStripMenuItem});
            this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
            this.groupsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.groupsToolStripMenuItem.Text = "Groups";
            // 
            // activateGroupsToolStripMenuItem
            // 
            this.activateGroupsToolStripMenuItem.Name = "activateGroupsToolStripMenuItem";
            this.activateGroupsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.activateGroupsToolStripMenuItem.Text = "Activate Groups";
            // 
            // monitorToolStripMenuItem
            // 
            this.monitorToolStripMenuItem.Name = "monitorToolStripMenuItem";
            this.monitorToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.monitorToolStripMenuItem.Text = "Monitor...";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(155, 6);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.configureToolStripMenuItem.Text = "Configure...";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
            // 
            // gradesToolStripMenuItem
            // 
            this.gradesToolStripMenuItem.Name = "gradesToolStripMenuItem";
            this.gradesToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.gradesToolStripMenuItem.Text = "Grades...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
            // 
            // mnuOperationsRegistration
            // 
            this.mnuOperationsRegistration.Name = "mnuOperationsRegistration";
            this.mnuOperationsRegistration.Size = new System.Drawing.Size(150, 22);
            this.mnuOperationsRegistration.Text = "Registration...";
            this.mnuOperationsRegistration.Click += new System.EventHandler(this.mnuOperationsRegistration_Click);
            // 
            // connectionsToolStripMenuItem
            // 
            this.connectionsToolStripMenuItem.Name = "connectionsToolStripMenuItem";
            this.connectionsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.connectionsToolStripMenuItem.Text = "Connections...";
            this.connectionsToolStripMenuItem.Click += new System.EventHandler(this.connectionsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences...";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolboxToolStripMenuItem,
            this.controlsToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolboxToolStripMenuItem
            // 
            this.toolboxToolStripMenuItem.Name = "toolboxToolStripMenuItem";
            this.toolboxToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.toolboxToolStripMenuItem.Text = "Toolbox";
            this.toolboxToolStripMenuItem.Click += new System.EventHandler(this.toolboxToolStripMenuItem_Click);
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.controlsToolStripMenuItem.Text = "Controls";
            this.controlsToolStripMenuItem.Click += new System.EventHandler(this.controlsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addonMarketplaceToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // addonMarketplaceToolStripMenuItem
            // 
            this.addonMarketplaceToolStripMenuItem.Name = "addonMarketplaceToolStripMenuItem";
            this.addonMarketplaceToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.addonMarketplaceToolStripMenuItem.Text = "Add-on Marketplace...";
            this.addonMarketplaceToolStripMenuItem.Click += new System.EventHandler(this.addonMarketplaceToolStripMenuItem_Click);
            // 
            // drawingTimer
            // 
            this.drawingTimer.Interval = 1;
            this.drawingTimer.Tick += new System.EventHandler(this.drawingTimer_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlCanvas);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(876, 403);
            this.panel1.TabIndex = 2;
            // 
            // pnlCanvas
            // 
            this.pnlCanvas.BackColor = System.Drawing.Color.White;
            this.pnlCanvas.ContextMenuStrip = this.contextCanvas;
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 0);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(876, 403);
            this.pnlCanvas.TabIndex = 1;
            this.pnlCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanvas_Paint);
            this.pnlCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseDown);
            this.pnlCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseMove);
            this.pnlCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlCanvas_MouseUp);
            this.pnlCanvas.Resize += new System.EventHandler(this.pnlCanvas_Resize);
            // 
            // contextCanvas
            // 
            this.contextCanvas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.penToolStripMenuItem,
            this.eraserToolStripMenuItem1,
            this.toolStripSeparator7,
            this.insertToolStripMenuItem,
            this.toolStripSeparator8,
            this.navigateToolStripMenuItem});
            this.contextCanvas.Name = "contextCanvas";
            this.contextCanvas.Size = new System.Drawing.Size(122, 104);
            // 
            // penToolStripMenuItem
            // 
            this.penToolStripMenuItem.Name = "penToolStripMenuItem";
            this.penToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.penToolStripMenuItem.Text = "Pen";
            this.penToolStripMenuItem.Click += new System.EventHandler(this.penToolStripMenuItem_Click);
            // 
            // eraserToolStripMenuItem1
            // 
            this.eraserToolStripMenuItem1.Name = "eraserToolStripMenuItem1";
            this.eraserToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.eraserToolStripMenuItem1.Text = "Eraser";
            this.eraserToolStripMenuItem1.Click += new System.EventHandler(this.eraserToolStripMenuItem1_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(118, 6);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextPanelInsertTextbox});
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.insertToolStripMenuItem.Text = "Insert";
            // 
            // contextPanelInsertTextbox
            // 
            this.contextPanelInsertTextbox.Name = "contextPanelInsertTextbox";
            this.contextPanelInsertTextbox.Size = new System.Drawing.Size(127, 22);
            this.contextPanelInsertTextbox.Text = "Text Box...";
            this.contextPanelInsertTextbox.Click += new System.EventHandler(this.contextPanelInsertTextbox_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(118, 6);
            // 
            // navigateToolStripMenuItem
            // 
            this.navigateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstPageToolStripMenuItem,
            this.nextPageToolStripMenuItem,
            this.previousPageToolStripMenuItem,
            this.lastPageToolStripMenuItem});
            this.navigateToolStripMenuItem.Name = "navigateToolStripMenuItem";
            this.navigateToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.navigateToolStripMenuItem.Text = "Navigate";
            // 
            // firstPageToolStripMenuItem
            // 
            this.firstPageToolStripMenuItem.Name = "firstPageToolStripMenuItem";
            this.firstPageToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.firstPageToolStripMenuItem.Text = "First";
            // 
            // nextPageToolStripMenuItem
            // 
            this.nextPageToolStripMenuItem.Name = "nextPageToolStripMenuItem";
            this.nextPageToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.nextPageToolStripMenuItem.Text = "Next";
            // 
            // previousPageToolStripMenuItem
            // 
            this.previousPageToolStripMenuItem.Name = "previousPageToolStripMenuItem";
            this.previousPageToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.previousPageToolStripMenuItem.Text = "Previous";
            // 
            // lastPageToolStripMenuItem
            // 
            this.lastPageToolStripMenuItem.Name = "lastPageToolStripMenuItem";
            this.lastPageToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.lastPageToolStripMenuItem.Text = "Last";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(876, 427);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.contextCanvas.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuPen;
        private System.Windows.Forms.ToolStripMenuItem mnuPenColor;
        private System.Windows.Forms.ToolStripMenuItem mnuPenColorBlack;
        private System.Windows.Forms.ToolStripMenuItem mnuPenColorRed;
        private System.Windows.Forms.ToolStripMenuItem mnuPenColorGreen;
        private System.Windows.Forms.ToolStripMenuItem mnuPenColorBlue;
        private System.Windows.Forms.ToolStripComboBox cmbPenWidth;
        private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eraserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOperationsRegistration;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boardSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quizToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pollingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startQuizToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pollToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem gradesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuNewClass;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem importRollsheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem groupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activateGroupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.Timer drawingTimer;
        private System.Windows.Forms.ToolStripMenuItem connectionsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private Canvas pnlCanvas;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextCanvas;
        private System.Windows.Forms.ToolStripMenuItem penToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eraserToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contextPanelInsertTextbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem navigateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addonMarketplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitorToolStripMenuItem;
    }
}

