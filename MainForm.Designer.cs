namespace Ketarin
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imlStatus = new System.Windows.Forms.ImageList(this.components);
            // Replaced ContextMenu with ContextMenuStrip
            this.cmnuJobs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuForceDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuInstall = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuUpdateInstall = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuCommands = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuRunPostDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInvert = new System.Windows.Forms.ToolStripMenuItem();
            // Replaced MainMenu with MenuStrip
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExportAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnusep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTutorial = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuUpdateAndInstall = new System.Windows.Forms.ToolStripMenuItem();
            this.cmuAdd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuImportFile = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuImportOnline = new System.Windows.Forms.ToolStripMenuItem();
            this.olvJobs = new Ketarin.ApplicationJobsListView();
            this.colName = new CDBurnerXP.Controls.OLVColumn();
            this.colLastUpdate = new CDBurnerXP.Controls.OLVColumn();
            this.colProgress = new CDBurnerXP.Controls.OLVColumn();
            this.colTarget = new CDBurnerXP.Controls.OLVColumn();
            this.colCategory = new CDBurnerXP.Controls.OLVColumn();
            this.colStatus = new CDBurnerXP.Controls.OLVColumn();
            // Removed VistaMenu component as it's not compatible with .NET 9
            // this.m_VistaMenu = new CDBurnerXP.Controls.VistaMenu(this.components);
            this.cmuRun = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuCheckAndDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuOnlyCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.ntiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmnuTrayIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tbSelectedApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbNumByStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbTotalApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.bInstall = new wyDay.Controls.SplitButton();
            this.bRun = new wyDay.Controls.SplitButton();
            this.bAddApplication = new wyDay.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).BeginInit();
            // ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).BeginInit(); // Removed VistaMenu
            this.statusBar.SuspendLayout();
            this.mnuMain.SuspendLayout(); // Added MenuStrip
            this.cmnuJobs.SuspendLayout(); // Added ContextMenuStrip
            this.cmuAdd.SuspendLayout(); // Added ContextMenuStrip
            this.cmuRun.SuspendLayout(); // Added ContextMenuStrip
            this.cmnuTrayIconMenu.SuspendLayout(); // Added ContextMenuStrip
            this.SuspendLayout();
            // 
            // imlStatus
            // 
            this.imlStatus.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlStatus.ImageSize = new System.Drawing.Size(16, 16);
            this.imlStatus.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmnuJobs (ContextMenuStrip)
            // 
            this.cmnuJobs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuUpdate,
            this.cmnuCheckForUpdate,
            this.cmnuForceDownload,
            this.menuItem2,
            this.cmnuInstall,
            this.cmnuUpdateInstall,
            this.menuItem1,
            this.cmnuCommands,
            this.menuItem5,
            this.cmnuOpenFile,
            this.cmnuOpenFolder,
            this.cmnuProperties,
            this.cmnuRename,
            this.menuItem4,
            this.cmnuEdit,
            this.cmnuDelete,
            this.cmnuCopy,
            this.cmnuPaste,
            this.mnuSelectAll,
            this.mnuInvert});
            this.cmnuJobs.Name = "cmnuJobs";
            this.cmnuJobs.Size = new System.Drawing.Size(181, 352);
            this.cmnuJobs.Opening += new System.ComponentModel.CancelEventHandler(this.cmnuJobs_Popup);
            // 
            // cmnuUpdate
            // 
            // Removed VistaMenu image setting
            // this.m_VistaMenu.SetImage(this.cmnuUpdate, global::Ketarin.Properties.Resources.Restart);
            this.cmnuUpdate.Name = "cmnuUpdate";
            this.cmnuUpdate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.cmnuUpdate.Size = new System.Drawing.Size(180, 22);
            this.cmnuUpdate.Text = "&Update";
            this.cmnuUpdate.Click += new System.EventHandler(this.cmuUpdate_Click);
            // 
            // cmnuCheckForUpdate
            // 
            this.cmnuCheckForUpdate.Name = "cmnuCheckForUpdate";
            this.cmnuCheckForUpdate.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
            this.cmnuCheckForUpdate.Size = new System.Drawing.Size(180, 22);
            this.cmnuCheckForUpdate.Text = "C&heck for update";
            this.cmnuCheckForUpdate.Click += new System.EventHandler(this.cmnuCheckForUpdate_Click);
            // 
            // cmnuForceDownload
            // 
            this.cmnuForceDownload.Name = "cmnuForceDownload";
            this.cmnuForceDownload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.cmnuForceDownload.Size = new System.Drawing.Size(180, 22);
            this.cmnuForceDownload.Text = "&Force download";
            this.cmnuForceDownload.Click += new System.EventHandler(this.cmnuForceDownload_Click);
            // 
            // menuItem2 (Separator)
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // cmnuInstall
            // 
            this.cmnuInstall.Name = "cmnuInstall";
            this.cmnuInstall.Size = new System.Drawing.Size(180, 22);
            this.cmnuInstall.Text = "&Install";
            this.cmnuInstall.Click += new System.EventHandler(this.cmnuInstall_Click);
            // 
            // cmnuUpdateInstall
            // 
            this.cmnuUpdateInstall.Name = "cmnuUpdateInstall";
            this.cmnuUpdateInstall.Size = new System.Drawing.Size(180, 22);
            this.cmnuUpdateInstall.Text = "Upda&te and install";
            this.cmnuUpdateInstall.Click += new System.EventHandler(this.cmnuUpdateInstall_Click);
            // 
            // menuItem1 (Separator)
            // 
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // cmnuCommands
            // 
            this.cmnuCommands.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuRunPostDownload});
            this.cmnuCommands.Name = "cmnuCommands";
            this.cmnuCommands.Size = new System.Drawing.Size(180, 22);
            this.cmnuCommands.Text = "Com&mands";
            // 
            // cmnuRunPostDownload
            // 
            this.cmnuRunPostDownload.Name = "cmnuRunPostDownload";
            this.cmnuRunPostDownload.Size = new System.Drawing.Size(224, 22);
            this.cmnuRunPostDownload.Text = "&Run post-download command";
            this.cmnuRunPostDownload.Click += new System.EventHandler(this.cmnuRunPostDownload_Click);
            // 
            // menuItem5 (Separator)
            // 
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(177, 6);
            // 
            // cmnuOpenFile
            // 
            this.cmnuOpenFile.Enabled = false;
            this.cmnuOpenFile.Name = "cmnuOpenFile";
            this.cmnuOpenFile.Size = new System.Drawing.Size(180, 22);
            this.cmnuOpenFile.Text = "&Open file";
            this.cmnuOpenFile.Click += new System.EventHandler(this.cmnuOpenFile_Click);
            // 
            // cmnuOpenFolder
            // 
            this.cmnuOpenFolder.Name = "cmnuOpenFolder";
            this.cmnuOpenFolder.Size = new System.Drawing.Size(180, 22);
            this.cmnuOpenFolder.Text = "Ope&n folder";
            this.cmnuOpenFolder.Click += new System.EventHandler(this.cmnuOpenFolder_Click);
            // 
            // cmnuProperties
            // 
            this.cmnuProperties.Enabled = false;
            this.cmnuProperties.Name = "cmnuProperties";
            this.cmnuProperties.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.cmnuProperties.Size = new System.Drawing.Size(180, 22);
            this.cmnuProperties.Text = "File propertie&s";
            this.cmnuProperties.Click += new System.EventHandler(this.cmnuProperties_Click);
            // 
            // cmnuRename
            // 
            this.cmnuRename.Enabled = false;
            this.cmnuRename.Name = "cmnuRename";
            this.cmnuRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.cmnuRename.Size = new System.Drawing.Size(180, 22);
            this.cmnuRename.Text = "&Rename file";
            this.cmnuRename.Click += new System.EventHandler(this.cmnuRename_Click);
            // 
            // menuItem4 (Separator)
            // 
            this.menuItem4.Name = "menuItem4";
            this.menuItem4.Size = new System.Drawing.Size(177, 6);
            // 
            // cmnuEdit
            // 
            this.cmnuEdit.Enabled = false;
            this.cmnuEdit.Name = "cmnuEdit";
            this.cmnuEdit.Size = new System.Drawing.Size(180, 22);
            this.cmnuEdit.Text = "&Edit";
            this.cmnuEdit.Click += new System.EventHandler(this.cmnuEdit_Click);
            // 
            // cmnuDelete
            // 
            this.cmnuDelete.Enabled = false;
            this.cmnuDelete.Name = "cmnuDelete";
            this.cmnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.cmnuDelete.Size = new System.Drawing.Size(180, 22);
            this.cmnuDelete.Text = "&Delete";
            this.cmnuDelete.Click += new System.EventHandler(this.cmnuDelete_Click);
            // 
            // cmnuCopy
            // 
            this.cmnuCopy.Name = "cmnuCopy";
            this.cmnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.cmnuCopy.Size = new System.Drawing.Size(180, 22);
            this.cmnuCopy.Text = "&&Copy";
            this.cmnuCopy.Click += new System.EventHandler(this.cmnuCopy_Click);
            // 
            // cmnuPaste
            // 
            this.cmnuPaste.Name = "cmnuPaste";
            this.cmnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.cmnuPaste.Size = new System.Drawing.Size(180, 22);
            this.cmnuPaste.Text = "&Paste";
            this.cmnuPaste.Click += new System.EventHandler(this.cmnuPaste_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuSelectAll.Size = new System.Drawing.Size(180, 22);
            this.mnuSelectAll.Text = "Select &all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuInvert
            // 
            this.mnuInvert.Name = "mnuInvert";
            this.mnuInvert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mnuInvert.Size = new System.Drawing.Size(180, 22);
            this.mnuInvert.Text = "In&vert selection";
            this.mnuInvert.Click += new System.EventHandler(this.mnuInvert_Click);
            // 
            // mnuMain (MenuStrip)
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(682, 24);
            this.mnuMain.TabIndex = 7;
            this.mnuMain.Text = "mnuMain";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuImport,
            this.mnuExportSelected,
            this.mnuExportAll,
            this.mnusep2,
            this.mnuSettings,
            this.menuItem7,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(180, 22);
            this.mnuNew.Text = "&New application";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuImport
            // 
            this.mnuImport.Name = "mnuImport";
            this.mnuImport.Size = new System.Drawing.Size(180, 22);
            this.mnuImport.Text = "&Import applications...";
            this.mnuImport.Click += new System.EventHandler(this.mnuImport_Click);
            // 
            // mnuExportSelected
            // 
            this.mnuExportSelected.Name = "mnuExportSelected";
            this.mnuExportSelected.Size = new System.Drawing.Size(180, 22);
            this.mnuExportSelected.Text = "E&xport selected applications...";
            this.mnuExportSelected.Click += new System.EventHandler(this.mnuExportSelected_Click);
            // 
            // mnuExportAll
            // 
            this.mnuExportAll.Name = "mnuExportAll";
            this.mnuExportAll.Size = new System.Drawing.Size(180, 22);
            this.mnuExportAll.Text = "Ex&port all applications...";
            this.mnuExportAll.Click += new System.EventHandler(this.mnuExportAll_Click);
            // 
            // mnusep2 (Separator)
            // 
            this.mnusep2.Name = "mnusep2";
            this.mnusep2.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(180, 22);
            this.mnuSettings.Text = "&Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // menuItem7 (Separator)
            // 
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(180, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLog,
            this.mnuShowGroups,
            this.mnuShowStatusBar,
            this.mnuAutoScroll,
            this.mnuFind});
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(44, 20);
            this.mnuView.Text = "&View";
            // 
            // mnuLog
            // 
            this.mnuLog.Name = "mnuLog";
            this.mnuLog.Size = new System.Drawing.Size(180, 22);
            this.mnuLog.Text = "&Log";
            this.mnuLog.Click += new System.EventHandler(this.mnuLog_Click);
            // 
            // mnuShowGroups
            // 
            this.mnuShowGroups.Name = "mnuShowGroups";
            this.mnuShowGroups.Size = new System.Drawing.Size(180, 22);
            this.mnuShowGroups.Text = "Show &groups";
            this.mnuShowGroups.Click += new System.EventHandler(this.mnuShowGroups_Click);
            // 
            // mnuShowStatusBar
            // 
            this.mnuShowStatusBar.Name = "mnuShowStatusBar";
            this.mnuShowStatusBar.Size = new System.Drawing.Size(180, 22);
            this.mnuShowStatusBar.Text = "Show &status bar";
            this.mnuShowStatusBar.Click += new System.EventHandler(this.mnuShowStatusBar_Click);
            // 
            // mnuAutoScroll
            // 
            this.mnuAutoScroll.Name = "mnuAutoScroll";
            this.mnuAutoScroll.Size = new System.Drawing.Size(180, 22);
            this.mnuAutoScroll.Text = "&Auto scroll";
            this.mnuAutoScroll.Click += new System.EventHandler(this.mnuAutoScroll_Click);
            // 
            // mnuFind
            // 
            this.mnuFind.Name = "mnuFind";
            this.mnuFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.mnuFind.Size = new System.Drawing.Size(180, 22);
            this.mnuFind.Text = "&Find";
            this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTutorial,
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuTutorial
            // 
            this.mnuTutorial.Name = "mnuTutorial";
            this.mnuTutorial.Size = new System.Drawing.Size(180, 22);
            this.mnuTutorial.Text = "&Tutorial";
            this.mnuTutorial.Click += new System.EventHandler(this.mnuTutorial_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // cmnuUpdateAndInstall
            // 
            this.cmnuUpdateAndInstall.Name = "cmnuUpdateAndInstall";
            this.cmnuUpdateAndInstall.Size = new System.Drawing.Size(180, 22);
            this.cmnuUpdateAndInstall.Text = "Upda&te and install";
            this.cmnuUpdateAndInstall.Click += new System.EventHandler(this.cmnuUpdateAndInstall_Click);
            // 
            // cmuAdd (ContextMenuStrip)
            // 
            this.cmuAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuAdd,
            this.cmnuImportFile,
            this.cmnuImportOnline});
            this.cmuAdd.Name = "cmuAdd";
            this.cmuAdd.Size = new System.Drawing.Size(181, 70);
            // 
            // cmnuAdd
            // 
            this.cmnuAdd.Name = "cmnuAdd";
            this.cmnuAdd.Size = new System.Drawing.Size(180, 22);
            this.cmnuAdd.Text = "&Add application";
            this.cmnuAdd.Click += new System.EventHandler(this.cmnuAdd_Click);
            // 
            // cmnuImportFile
            // 
            this.cmnuImportFile.Name = "cmnuImportFile";
            this.cmnuImportFile.Size = new System.Drawing.Size(180, 22);
            this.cmnuImportFile.Text = "Import from &file...";
            this.cmnuImportFile.Click += new System.EventHandler(this.cmnuImport_Click);
            // 
            // cmnuImportOnline
            // 
            this.cmnuImportOnline.Name = "cmnuImportOnline";
            this.cmnuImportOnline.Size = new System.Drawing.Size(180, 22);
            this.cmnuImportOnline.Text = "Import from &online database...";
            this.cmnuImportOnline.Click += new System.EventHandler(this.cmnuImportOnline_Click);
            // 
            // olvJobs (ApplicationJobsListView)
            // 
            this.olvJobs.AllColumns.Add(this.colName);
            this.olvJobs.AllColumns.Add(this.colLastUpdate);
            this.olvJobs.AllColumns.Add(this.colProgress);
            this.olvJobs.AllColumns.Add(this.colTarget);
            this.olvJobs.AllColumns.Add(this.colCategory);
            this.olvJobs.AllColumns.Add(this.colStatus);
            this.olvJobs.AllowColumnReorder = true;
            this.olvJobs.AllowDrop = true;
            this.olvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvJobs.ContextMenuStrip = this.cmnuJobs;
            this.olvJobs.EmptyListMsg = "No applications defined yet";
            this.olvJobs.FullRowSelect = true;
            this.olvJobs.HideSelection = false;
            this.olvJobs.Location = new System.Drawing.Point(0, 24);
            this.olvJobs.Name = "olvJobs";
            this.olvJobs.ShowGroups = false;
            this.olvJobs.Size = new System.Drawing.Size(682, 357);
            this.olvJobs.TabIndex = 0;
            this.olvJobs.UseCompatibleStateImageBehavior = false;
            this.olvJobs.View = System.Windows.Forms.View.Details;
            this.olvJobs.SelectionChanged += new System.EventHandler(this.olvJobs_SelectionChanged);
            this.olvJobs.SelectedIndexChanged += new System.EventHandler(this.olvJobs_SelectedIndexChanged);
            this.olvJobs.DoubleClick += new System.EventHandler(this.olvJobs_DoubleClick);
            this.olvJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.olvJobs_DragDrop);
            this.olvJobs.DragEnter += new System.Windows.Forms.DragEventHandler(this.olvJobs_DragEnter);
            this.olvJobs.FilterChanged += new System.EventHandler(this.olvJobs_FilterChanged);
            this.olvJobs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvJobs_KeyDown);
            // 
            // colName (OLVColumn)
            // 
            this.colName.AspectName = "Name";
            this.colName.Text = "Application";
            this.colName.Width = 150;
            // 
            // colLastUpdate (OLVColumn)
            // 
            this.colLastUpdate.AspectName = "LastUpdated";
            this.colLastUpdate.Text = "Last updated";
            this.colLastUpdate.Width = 120;
            // 
            // colProgress (OLVColumn)
            // 
            this.colProgress.Text = "Progress";
            this.colProgress.Width = 100;
            // 
            // colTarget (OLVColumn)
            // 
            this.colTarget.FillsFreeSpace = true;
            this.colTarget.Text = "Target";
            this.colTarget.Width = 150;
            // 
            // colCategory (OLVColumn)
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 100;
            // 
            // colStatus (OLVColumn)
            // 
            this.colStatus.Text = "Status";
            // 
            // cmuRun (ContextMenuStrip)
            // 
            this.cmuRun.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuCheckAndDownload,
            this.cmnuOnlyCheck});
            this.cmuRun.Name = "cmuRun";
            this.cmuRun.Size = new System.Drawing.Size(181, 48);
            // 
            // cmnuCheckAndDownload
            // 
            this.cmnuCheckAndDownload.Name = "cmnuCheckAndDownload";
            this.cmnuCheckAndDownload.Size = new System.Drawing.Size(180, 22);
            this.cmnuCheckAndDownload.Text = "&Check && download";
            this.cmnuCheckAndDownload.Click += new System.EventHandler(this.cmnuCheckAndDownload_Click);
            // 
            // cmnuOnlyCheck
            // 
            this.cmnuOnlyCheck.Name = "cmnuOnlyCheck";
            this.cmnuOnlyCheck.Size = new System.Drawing.Size(180, 22);
            this.cmnuOnlyCheck.Text = "Only &check";
            this.cmnuOnlyCheck.Click += new System.EventHandler(this.cmnuOnlyCheck_Click);
            // 
            // ntiTrayIcon (NotifyIcon)
            // 
            this.ntiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiTrayIcon.Icon")));
            this.ntiTrayIcon.Text = "Ketarin (Idle)";
            this.ntiTrayIcon.Visible = true;
            this.ntiTrayIcon.DoubleClick += new System.EventHandler(this.ntiTrayIcon_DoubleClick);
            // 
            // cmnuTrayIconMenu (ContextMenuStrip)
            // 
            this.cmnuTrayIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuShow,
            this.cmnuExit});
            this.cmnuTrayIconMenu.Name = "cmnuTrayIconMenu";
            this.cmnuTrayIconMenu.Size = new System.Drawing.Size(181, 48);
            // 
            // cmnuShow
            // 
            this.cmnuShow.Name = "cmnuShow";
            this.cmnuShow.Size = new System.Drawing.Size(180, 22);
            this.cmnuShow.Text = "&Show";
            this.cmnuShow.Click += new System.EventHandler(this.cmnuShow_Click);
            // 
            // cmnuExit
            // 
            this.cmnuExit.Name = "cmnuExit";
            this.cmnuExit.Size = new System.Drawing.Size(180, 22);
            this.cmnuExit.Text = "E&xit";
            this.cmnuExit.Click += new System.EventHandler(this.cmnuExit_Click);
            // 
            // statusBar (StatusStrip)
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSelectedApplications,
            this.tbNumByStatus,
            this.tbTotalApplications});
            this.statusBar.Location = new System.Drawing.Point(0, 404);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(682, 22);
            this.statusBar.TabIndex = 8;
            this.statusBar.Text = "statusStrip1";
            // 
            // tbSelectedApplications
            // 
            this.tbSelectedApplications.Name = "tbSelectedApplications";
            this.tbSelectedApplications.Size = new System.Drawing.Size(118, 17);
            this.tbSelectedApplications.Text = "toolStripStatusLabel1";
            // 
            // tbNumByStatus
            // 
            this.tbNumByStatus.Name = "tbNumByStatus";
            this.tbNumByStatus.Size = new System.Drawing.Size(118, 17);
            this.tbNumByStatus.Text = "toolStripStatusLabel2";
            // 
            // tbTotalApplications
            // 
            this.tbTotalApplications.Name = "tbTotalApplications";
            this.tbTotalApplications.Size = new System.Drawing.Size(118, 17);
            this.tbTotalApplications.Text = "toolStripStatusLabel3";
            // 
            // bInstall (SplitButton)
            // 
            this.bInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bInstall.Location = new System.Drawing.Point(174, 384);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(75, 23);
            this.bInstall.SplitMenuStrip = null;
            this.bInstall.TabIndex = 6;
            this.bInstall.Text = "&Install";
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);
            // 
            // bRun (SplitButton)
            // 
            this.bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRun.Location = new System.Drawing.Point(93, 384);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(75, 23);
            this.bRun.SplitMenuStrip = this.cmuRun;
            this.bRun.TabIndex = 5;
            this.bRun.Text = "&Update all";
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // bAddApplication (SplitButton)
            // 
            this.bAddApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddApplication.Location = new System.Drawing.Point(12, 384);
            this.bAddApplication.Name = "bAddApplication";
            this.bAddApplication.Size = new System.Drawing.Size(75, 23);
            this.bAddApplication.SplitMenuStrip = this.cmuAdd;
            this.bAddApplication.TabIndex = 4;
            this.bAddApplication.Text = "&Add";
            this.bAddApplication.UseVisualStyleBackColor = true;
            this.bAddApplication.Click += new System.EventHandler(this.sbAddApplication_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 426);
            this.Controls.Add(this.bInstall);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.bAddApplication);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.olvJobs);
            this.Controls.Add(this.mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.Name = "MainForm";
            this.Text = "Ketarin";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.cmnuJobs.ResumeLayout(false);
            this.cmuAdd.ResumeLayout(false);
            this.cmuRun.ResumeLayout(false);
            this.cmnuTrayIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imlStatus;
        private System.Windows.Forms.ContextMenuStrip cmnuJobs;
        private System.Windows.Forms.ToolStripMenuItem cmnuUpdate;
        private System.Windows.Forms.ToolStripMenuItem cmnuCheckForUpdate;
        private System.Windows.Forms.ToolStripMenuItem cmnuForceDownload;
        private System.Windows.Forms.ToolStripSeparator menuItem2;
        private System.Windows.Forms.ToolStripMenuItem cmnuInstall;
        private System.Windows.Forms.ToolStripMenuItem cmnuUpdateInstall;
        private System.Windows.Forms.ToolStripSeparator menuItem1;
        private System.Windows.Forms.ToolStripMenuItem cmnuCommands;
        private System.Windows.Forms.ToolStripMenuItem cmnuRunPostDownload;
        private System.Windows.Forms.ToolStripSeparator menuItem5;
        private System.Windows.Forms.ToolStripMenuItem cmnuOpenFile;
        private System.Windows.Forms.ToolStripMenuItem cmnuOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem cmnuProperties;
        private System.Windows.Forms.ToolStripMenuItem cmnuRename;
        private System.Windows.Forms.ToolStripSeparator menuItem4;
        private System.Windows.Forms.ToolStripMenuItem cmnuEdit;
        private System.Windows.Forms.ToolStripMenuItem cmnuDelete;
        private System.Windows.Forms.ToolStripMenuItem cmnuCopy;
        private System.Windows.Forms.ToolStripMenuItem cmnuPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnuInvert;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuImport;
        private System.Windows.Forms.ToolStripMenuItem mnuExportSelected;
        private System.Windows.Forms.ToolStripMenuItem mnuExportAll;
        private System.Windows.Forms.ToolStripSeparator mnusep2;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.ToolStripSeparator menuItem7;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuLog;
        private System.Windows.Forms.ToolStripMenuItem mnuShowGroups;
        private System.Windows.Forms.ToolStripMenuItem mnuShowStatusBar;
        private System.Windows.Forms.ToolStripMenuItem mnuAutoScroll;
        private System.Windows.Forms.ToolStripMenuItem mnuFind;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuTutorial;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem cmnuUpdateAndInstall;
        private System.Windows.Forms.ContextMenuStrip cmuAdd;
        private System.Windows.Forms.ToolStripMenuItem cmnuAdd;
        private System.Windows.Forms.ToolStripMenuItem cmnuImportFile;
        private System.Windows.Forms.ToolStripMenuItem cmnuImportOnline;
        private ApplicationJobsListView olvJobs;
        private CDBurnerXP.Controls.OLVColumn colName;
        private CDBurnerXP.Controls.OLVColumn colLastUpdate;
        private CDBurnerXP.Controls.OLVColumn colProgress;
        private CDBurnerXP.Controls.OLVColumn colTarget;
        private CDBurnerXP.Controls.OLVColumn colCategory;
        private CDBurnerXP.Controls.OLVColumn colStatus;
        // Removed VistaMenu component as it's not compatible with .NET 9
        // private CDBurnerXP.Controls.VistaMenu m_VistaMenu;
        private System.Windows.Forms.ContextMenuStrip cmuRun;
        private System.Windows.Forms.ToolStripMenuItem cmnuCheckAndDownload;
        private System.Windows.Forms.ToolStripMenuItem cmnuOnlyCheck;
        private System.Windows.Forms.NotifyIcon ntiTrayIcon;
        private System.Windows.Forms.ContextMenuStrip cmnuTrayIconMenu;
        private System.Windows.Forms.ToolStripMenuItem cmnuShow;
        private System.Windows.Forms.ToolStripMenuItem cmnuExit;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tbSelectedApplications;
        private System.Windows.Forms.ToolStripStatusLabel tbNumByStatus;
        private System.Windows.Forms.ToolStripStatusLabel tbTotalApplications;
        private wyDay.Controls.SplitButton bInstall;
        private wyDay.Controls.SplitButton bRun;
        private wyDay.Controls.SplitButton bAddApplication;
    }
}

