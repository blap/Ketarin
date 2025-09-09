namespace Ketarin.Forms
{
    partial class CommandControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mnuBatchScript = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSScript = new System.Windows.Forms.ToolStripMenuItem();
            this.sepRun = new System.Windows.Forms.ToolStripSeparator();
            this.mnuValidate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSnippets = new System.Windows.Forms.ToolStripSeparator();
            this.mnuInsertSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewScript = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSaveAs = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDeleteSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.sepDefaultCommands = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.sepClipboard = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.sepSelection = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.txtBorder = new System.Windows.Forms.TextBox();
            this.bCommand = new wyDay.Controls.SplitButton();
            // Replaced Scintilla with TextBox and ContextMenu with ContextMenuStrip
            this.txtCode = new System.Windows.Forms.TextBox();
            this.cmnuCommand = new System.Windows.Forms.ContextMenuStrip();
            this.mnuPowerShell = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmnuCommand (ContextMenuStrip)
            // 
            this.cmnuCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBatchScript,
            this.mnuCSScript,
            this.mnuPowerShell,
            this.sepRun,
            this.mnuValidate,
            this.mnuRun,
            this.sepSnippets,
            this.mnuInsertSnippet,
            this.mnuSaveAs,
            this.mnuDeleteSnippet,
            this.sepDefaultCommands,
            this.mnuUndo,
            this.mnuRedo,
            this.sepClipboard,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuClear,
            this.sepSelection,
            this.mnuSelectAll});
            this.cmnuCommand.Name = "cmnuCommand";
            this.cmnuCommand.Size = new System.Drawing.Size(181, 352);
            this.cmnuCommand.Opening += new System.ComponentModel.CancelEventHandler(this.cmnuCommand_Opening);
            // 
            // mnuBatchScript
            // 
            this.mnuBatchScript.Checked = true;
            this.mnuBatchScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuBatchScript.Name = "mnuBatchScript";
            this.mnuBatchScript.Size = new System.Drawing.Size(180, 22);
            this.mnuBatchScript.Text = "&Batch script";
            this.mnuBatchScript.Click += new System.EventHandler(this.mnuBatchScript_Click);
            // 
            // mnuCSScript
            // 
            this.mnuCSScript.Name = "mnuCSScript";
            this.mnuCSScript.Size = new System.Drawing.Size(180, 22);
            this.mnuCSScript.Text = "C&# script";
            this.mnuCSScript.Click += new System.EventHandler(this.mnuCSScript_Click);
            // 
            // mnuPowerShell
            // 
            this.mnuPowerShell.Name = "mnuPowerShell";
            this.mnuPowerShell.Size = new System.Drawing.Size(180, 22);
            this.mnuPowerShell.Text = "PowerShell script";
            this.mnuPowerShell.Click += new System.EventHandler(this.mnuPowerShell_Click);
            // 
            // sepRun (Separator)
            // 
            this.sepRun.Name = "sepRun";
            this.sepRun.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuValidate
            // 
            this.mnuValidate.Name = "mnuValidate";
            this.mnuValidate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.mnuValidate.Size = new System.Drawing.Size(180, 22);
            this.mnuValidate.Text = "&Validate";
            this.mnuValidate.Click += new System.EventHandler(this.mnuValidate_Click);
            // 
            // mnuRun
            // 
            this.mnuRun.Name = "mnuRun";
            this.mnuRun.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.mnuRun.Size = new System.Drawing.Size(180, 22);
            this.mnuRun.Text = "&Run";
            this.mnuRun.Click += new System.EventHandler(this.mnuRun_Click);
            // 
            // sepSnippets (Separator)
            // 
            this.sepSnippets.Name = "sepSnippets";
            this.sepSnippets.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuInsertSnippet
            // 
            this.mnuInsertSnippet.Name = "mnuInsertSnippet";
            this.mnuInsertSnippet.Size = new System.Drawing.Size(180, 22);
            this.mnuInsertSnippet.Text = "I&nsert snippet";
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewScript,
            this.sepSaveAs});
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveAs.Text = "&Save as";
            // 
            // mnuNewScript
            // 
            this.mnuNewScript.Name = "mnuNewScript";
            this.mnuNewScript.Size = new System.Drawing.Size(180, 22);
            this.mnuNewScript.Text = "&New...";
            this.mnuNewScript.Click += new System.EventHandler(this.mnuNewScript_Click);
            // 
            // sepSaveAs (Separator)
            // 
            this.sepSaveAs.Name = "sepSaveAs";
            this.sepSaveAs.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuDeleteSnippet
            // 
            this.mnuDeleteSnippet.Name = "mnuDeleteSnippet";
            this.mnuDeleteSnippet.Size = new System.Drawing.Size(180, 22);
            this.mnuDeleteSnippet.Text = "&Delete snippet";
            // 
            // sepDefaultCommands (Separator)
            // 
            this.sepDefaultCommands.Name = "sepDefaultCommands";
            this.sepDefaultCommands.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuUndo
            // 
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuUndo.Size = new System.Drawing.Size(180, 22);
            this.mnuUndo.Text = "&Undo";
            this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
            // 
            // mnuRedo
            // 
            this.mnuRedo.Name = "mnuRedo";
            this.mnuRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mnuRedo.Size = new System.Drawing.Size(180, 22);
            this.mnuRedo.Text = "R&edo";
            this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
            // 
            // sepClipboard (Separator)
            // 
            this.sepClipboard.Name = "sepClipboard";
            this.sepClipboard.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuCut
            // 
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuCut.Size = new System.Drawing.Size(180, 22);
            this.mnuCut.Text = "Cu&t";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCopy.Size = new System.Drawing.Size(180, 22);
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mnuPaste.Size = new System.Drawing.Size(180, 22);
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(180, 22);
            this.mnuClear.Text = "Cle&ar";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // sepSelection (Separator)
            // 
            this.sepSelection.Name = "sepSelection";
            this.sepSelection.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.Size = new System.Drawing.Size(180, 22);
            this.mnuSelectAll.Text = "Se&lect all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // txtBorder
            // 
            this.txtBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBorder.Location = new System.Drawing.Point(0, 0);
            this.txtBorder.Multiline = true;
            this.txtBorder.Name = "txtBorder";
            this.txtBorder.ReadOnly = true;
            this.txtBorder.Size = new System.Drawing.Size(517, 194);
            this.txtBorder.TabIndex = 2;
            this.txtBorder.TabStop = false;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.Location = new System.Drawing.Point(1, 1);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(514, 192);
            this.txtCode.TabIndex = 0;
            this.txtCode.WordWrap = false;
            // 
            // bCommand
            // 
            this.bCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCommand.AutoSize = true;
            this.bCommand.Location = new System.Drawing.Point(0, 200);
            this.bCommand.Name = "bCommand";
            this.bCommand.SeparateDropdownButton = false;
            this.bCommand.Size = new System.Drawing.Size(82, 23);
            this.bCommand.SplitMenuStrip = this.cmnuCommand;
            this.bCommand.TabIndex = 1;
            this.bCommand.Text = "&Command";
            this.bCommand.UseVisualStyleBackColor = true;
            // 
            // CommandControl
            // 
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.bCommand);
            this.Controls.Add(this.txtBorder);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CommandControl";
            this.Size = new System.Drawing.Size(517, 223);
            this.cmnuCommand.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private wyDay.Controls.SplitButton bCommand;
        // Replaced ContextMenu with ContextMenuStrip
        private System.Windows.Forms.ContextMenuStrip cmnuCommand;
        private System.Windows.Forms.ToolStripMenuItem mnuBatchScript;
        private System.Windows.Forms.ToolStripMenuItem mnuCSScript;
        private System.Windows.Forms.ToolStripSeparator sepRun;
        private System.Windows.Forms.ToolStripMenuItem mnuValidate;
        private System.Windows.Forms.ToolStripMenuItem mnuRun;
        private System.Windows.Forms.ToolStripSeparator sepSnippets;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertSnippet;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuNewScript;
        private System.Windows.Forms.ToolStripSeparator sepSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteSnippet;
        // Replaced Scintilla with TextBox
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.ToolStripSeparator sepDefaultCommands;
        private System.Windows.Forms.ToolStripMenuItem mnuUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuRedo;
        private System.Windows.Forms.ToolStripSeparator sepClipboard;
        private System.Windows.Forms.ToolStripMenuItem mnuCut;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuPaste;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
        private System.Windows.Forms.ToolStripSeparator sepSelection;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
        private System.Windows.Forms.TextBox txtBorder;
        private System.Windows.Forms.ToolStripMenuItem mnuPowerShell;
    }
}