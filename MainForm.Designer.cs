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
            this.olvJobs = new Ketarin.ApplicationJobsListView();
            this.colName = new CDBurnerXP.Controls.OLVColumn();
            this.colLastUpdate = new CDBurnerXP.Controls.OLVColumn();
            this.colProgress = new CDBurnerXP.Controls.OLVColumn();
            this.colTarget = new CDBurnerXP.Controls.OLVColumn();
            this.colCategory = new CDBurnerXP.Controls.OLVColumn();
            this.colStatus = new CDBurnerXP.Controls.OLVColumn();
            this.m_VistaMenu = new CDBurnerXP.Controls.VistaMenu(this.components);
            this.ntiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.tbSelectedApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbNumByStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbTotalApplications = new System.Windows.Forms.ToolStripStatusLabel();
            this.bInstall = new wyDay.Controls.SplitButton();
            this.bRun = new wyDay.Controls.SplitButton();
            this.bAddApplication = new wyDay.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();

            // Temporarily disabled menu controls for .NET 6.0 migration
            // All ContextMenu, MenuItem, and MainMenu controls are disabled

            //
            // imlStatus
            //
            this.imlStatus.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlStatus.ImageSize = new System.Drawing.Size(16, 16);
            this.imlStatus.TransparentColor = System.Drawing.Color.Transparent;

            //
            // olvJobs
            //
            this.olvJobs.AllColumns.Add(this.colName);
            this.olvJobs.AllColumns.Add(this.colLastUpdate);
            this.olvJobs.AllColumns.Add(this.colProgress);
            this.olvJobs.AllColumns.Add(this.colTarget);
            this.olvJobs.AllColumns.Add(this.colCategory);
            this.olvJobs.AllColumns.Add(this.colStatus);
            this.olvJobs.AllowColumnReorder = true;
            this.olvJobs.AlternateRowBackColor = System.Drawing.Color.Empty;
            this.olvJobs.AlwaysGroupByColumn = null;
            this.olvJobs.AlwaysGroupBySortOrder = System.Windows.Forms.SortOrder.None;
            this.olvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.olvJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colLastUpdate,
            this.colProgress,
            this.colTarget,
            this.colCategory});
            this.olvJobs.FullRowSelect = true;
            this.olvJobs.HideSelection = false;
            this.olvJobs.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.olvJobs.HighlightForegroundColor = System.Drawing.Color.Empty;
            this.olvJobs.LastSortColumn = null;
            this.olvJobs.LastSortOrder = System.Windows.Forms.SortOrder.None;
            this.olvJobs.Location = new System.Drawing.Point(12, 12);
            this.olvJobs.Name = "olvJobs";
            this.olvJobs.OwnerDraw = true;
            this.olvJobs.Size = new System.Drawing.Size(658, 262);
            this.olvJobs.SmallImageList = this.imlStatus;
            this.olvJobs.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.olvJobs.TabIndex = 0;
            this.olvJobs.UseCompatibleStateImageBehavior = false;
            this.olvJobs.View = System.Windows.Forms.View.Details;
            this.olvJobs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvJobs_KeyDown);
            this.olvJobs.SelectionChanged += new System.EventHandler(this.olvJobs_SelectionChanged);
            this.olvJobs.SelectedIndexChanged += new System.EventHandler(this.olvJobs_SelectedIndexChanged);
            this.olvJobs.DoubleClick += new System.EventHandler(this.olvJobs_DoubleClick);

            //
            // colName
            //
            this.colName.AspectName = null;
            this.colName.Text = "Application";
            this.colName.Width = 183;

            //
            // colLastUpdate
            //
            this.colLastUpdate.AspectName = null;
            this.colLastUpdate.Text = "Last updated";
            this.colLastUpdate.Width = 110;

            //
            // colProgress
            //
            this.colProgress.AspectName = null;
            this.colProgress.MaximumWidth = 100;
            this.colProgress.MinimumWidth = 100;
            this.colProgress.Text = "Progress";
            this.colProgress.Width = 100;

            //
            // colTarget
            //
            this.colTarget.AspectName = null;
            this.colTarget.FillsFreeSpace = true;
            this.colTarget.Text = "Target";

            //
            // colCategory
            //
            this.colCategory.AspectName = "Category";
            this.colCategory.Text = "Category";
            this.colCategory.Width = 80;

            //
            // colStatus
            //
            this.colStatus.AspectName = null;
            this.colStatus.IsVisible = false;
            this.colStatus.Text = "Status";
            this.colStatus.Width = 80;

            //
            // m_VistaMenu
            //
            this.m_VistaMenu.ContainerControl = this;

            //
            // ntiTrayIcon
            //
            this.ntiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiTrayIcon.Icon")));
            this.ntiTrayIcon.Text = "Ketarin (Idle)";
            this.ntiTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntiTrayIcon_MouseDoubleClick);

            //
            // statusBar
            //
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSelectedApplications,
            this.tbNumByStatus,
            this.tbTotalApplications});
            this.statusBar.Location = new System.Drawing.Point(0, 240);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(682, 24);
            this.statusBar.TabIndex = 6;
            this.statusBar.Text = "statusBar";
            this.statusBar.Visible = false;

            //
            // tbSelectedApplications
            //
            this.tbSelectedApplications.Name = "tbSelectedApplications";
            this.tbSelectedApplications.Size = new System.Drawing.Size(130, 19);
            this.tbSelectedApplications.Text = "Selected applications: 0";

            //
            // tbNumByStatus
            //
            this.tbNumByStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tbNumByStatus.Name = "tbNumByStatus";
            this.tbNumByStatus.Size = new System.Drawing.Size(197, 19);
            this.tbNumByStatus.Text = "By status: 0 Idle, 0 Finished, 0 Failed";

            //
            // tbTotalApplications
            //
            this.tbTotalApplications.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.tbTotalApplications.Name = "tbTotalApplications";
            this.tbTotalApplications.Size = new System.Drawing.Size(340, 19);
            this.tbTotalApplications.Spring = true;
            this.tbTotalApplications.Text = "Number of applications: 0";
            this.tbTotalApplications.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // bInstall
            //
            this.bInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bInstall.AutoSize = true;
            this.bInstall.Image = global::Ketarin.Properties.Resources.Setup;
            this.bInstall.Location = new System.Drawing.Point(290, 280);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(85, 24);
            this.bInstall.TabIndex = 5;
            this.bInstall.Text = "I&nstall...";
            this.bInstall.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bInstall.UseVisualStyleBackColor = true;
            this.bInstall.Click += new System.EventHandler(this.bInstall_Click);

            //
            // bRun
            //
            this.bRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRun.AutoSize = true;
            this.bRun.Image = global::Ketarin.Properties.Resources.Restart;
            this.bRun.Location = new System.Drawing.Point(168, 280);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(116, 24);
            this.bRun.TabIndex = 4;
            this.bRun.Text = "&Update all";
            this.bRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);

            //
            // bAddApplication
            //
            this.bAddApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAddApplication.AutoSize = true;
            this.bAddApplication.Image = global::Ketarin.Properties.Resources.AddSmall;
            this.bAddApplication.Location = new System.Drawing.Point(12, 280);
            this.bAddApplication.Name = "bAddApplication";
            this.bAddApplication.Size = new System.Drawing.Size(150, 24);
            this.bAddApplication.TabIndex = 3;
            this.bAddApplication.Text = "&Add new application";
            this.bAddApplication.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bAddApplication.UseVisualStyleBackColor = true;
            this.bAddApplication.Click += new System.EventHandler(this.sbAddApplication_Click);

            //
            // MainForm
            //
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 316);
            this.Controls.Add(this.bInstall);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.bAddApplication);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.olvJobs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            // this.Menu = this.mnuMain; // Temporarily disabled for .NET 6.0 migration
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.SavePosition = true;
            this.Text = "Ketarin";
            ((System.ComponentModel.ISupportInitialize)(this.olvJobs)).EndInit();
            // ((System.ComponentModel.ISupportInitialize)(this.m_VistaMenu)).EndInit(); // Temporarily disabled for .NET 6.0 migration
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ApplicationJobsListView olvJobs;
        private System.Windows.Forms.ImageList imlStatus;
        // private CDBurnerXP.Controls.VistaMenu m_VistaMenu; // Temporarily disabled for .NET 6.0 migration
        private CDBurnerXP.Controls.OLVColumn colName;
        private CDBurnerXP.Controls.OLVColumn colLastUpdate;
        private CDBurnerXP.Controls.OLVColumn colProgress;
        private CDBurnerXP.Controls.OLVColumn colTarget;
        private CDBurnerXP.Controls.OLVColumn colCategory;
        private CDBurnerXP.Controls.OLVColumn colStatus;
        private wyDay.Controls.SplitButton bAddApplication;
        private wyDay.Controls.SplitButton bRun;
        private wyDay.Controls.SplitButton bInstall;
        private System.Windows.Forms.NotifyIcon ntiTrayIcon;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel tbSelectedApplications;
        private System.Windows.Forms.ToolStripStatusLabel tbNumByStatus;
        private System.Windows.Forms.ToolStripStatusLabel tbTotalApplications;
    }
}

