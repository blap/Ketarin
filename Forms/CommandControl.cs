using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Ketarin.Database; // Add reference to our new database namespace
using wyDay.Controls;

namespace Ketarin.Forms
{
    public partial class CommandControl : UserControl
    {
        private const string csSample = @"/*
  Enter a custom C# script here. C# is case sensitive.
  ""app"" references the current application.
  Example:
  MessageBox.Show(app.Name);
  
  = Notable methods =
  app.PreviousLocation
    Corresponds to the variable {file}

  app.Variables.ReplaceAllInString(""Any {text} with variables."")
    Replaces all known variables in a given string.
    Example: string new = app.Variables.ReplaceAllInString(""{file}"")

  return;
    Exits the script.

  Abort(""Error text"");
    Exits the script with a given error.
*/";

        private const string psSample = "write-host $app.Name";

        private string[] variableNames = new string[0];

        public CommandControl()
        {
            InitializeComponent();
            
            // Configure the TextBox for code editing
            txtCode.Font = new Font(FontFamily.GenericMonospace, 10);
            txtCode.Multiline = true;
            txtCode.ScrollBars = ScrollBars.Both;
            txtCode.AcceptsTab = true;
            txtCode.WordWrap = false;
        }
        
        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        [DefaultValue("")]
        public new string Text
        {
            get
            {
                return txtCode.Text;
            }
            set
            {
                txtCode.Text = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets if a border should be displayed around the text control.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowBorder
        {
            get
            {
                return txtBorder.Visible;
            }
            set
            {
                if (value != txtCode.Visible)
                {
                    if (value)
                    {
                        txtCode.Bounds = new Rectangle(txtCode.Left + 1, txtCode.Top + 1, txtCode.Width - 2, txtCode.Height - 2);
                    }
                    else
                    {
                        txtCode.Bounds = new Rectangle(txtCode.Left - 1, txtCode.Top - 1, txtCode.Width + 2, txtCode.Height + 2);
                    }
                }
                txtBorder.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the control is in read only mode.
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            set
            {
                txtCode.ReadOnly = value;
                bCommand.Enabled = !value;
            }
            get
            {
                return txtCode.ReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the currently exiting variables for the current application.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] VariableNames
        {
            get { return this.variableNames; }
            set
            {
                this.variableNames = value;

                // Remove old menu items
                for (int i = cmnuCommand.Items.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(cmnuCommand.Items[i].Tag as string))
                    {
                        cmnuCommand.Items.RemoveAt(i);
                    }
                }

                // Add necessary menu items
                if (this.VariableNames != null && this.VariableNames.Length > 0)
                {
                    ToolStripSeparator varSeparator = new ToolStripSeparator();
                    varSeparator.Tag = "VarSeparator";
                    cmnuCommand.Items.Insert(0, varSeparator);

                    for (int i = this.VariableNames.Length - 1; i >= 0; i--)
                    {
                        ToolStripMenuItem varItem = new ToolStripMenuItem("{" + VariableNames[i] + "}");
                        varItem.Tag = VariableNames[i];
                        varItem.Click += (sender, ev) => {
                            if (sender is ToolStripMenuItem menuItem)
                            {
                                txtCode.SelectedText = menuItem.Text;
                            }
                        };
                        cmnuCommand.Items.Insert(0, varItem);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the application associated with this command control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ApplicationJob? Application { get; set; }

        /// <summary>
        /// Gets or sets whether to indent the button.
        /// </summary>
        [DefaultValue(0)]
        public int IndentButton
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of command.
        /// </summary>
        [DefaultValue(ScriptType.Batch), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ScriptType CommandType
        {
            get
            {
                if (mnuCSScript.Checked)
                {
                    return ScriptType.CS;
                }
                else if (mnuPowerShell.Checked)
                {
                    return ScriptType.PowerShell;
                }

                return ScriptType.Batch;
            }
            set
            {
                switch (value)
                {
                    case ScriptType.PowerShell:
                        mnuBatchScript.Checked = false;
                        mnuCSScript.Checked = false;
                        mnuPowerShell.Checked = true;
                        mnuValidate.Enabled = false;
                        // txtCode.LexerLanguage = "powershell"; // Removed Scintilla-specific property
                        if (string.IsNullOrEmpty(txtCode.Text))
                        {
                            txtCode.Text = psSample;
                        }
                        break;
                    case ScriptType.CS:
                        mnuBatchScript.Checked = false;
                        mnuCSScript.Checked = true;
                        mnuPowerShell.Checked = false;
                        mnuValidate.Enabled = true;
                        // txtCode.LexerLanguage = "cs"; // Removed Scintilla-specific property
                        if (string.IsNullOrEmpty(txtCode.Text))
                        {
                            txtCode.Text = csSample;
                        }
                        break;
                    default:
                        mnuBatchScript.Checked = true;
                        mnuCSScript.Checked = false;
                        mnuPowerShell.Checked = false;
                        mnuValidate.Enabled = false;
                        // txtCode.LexerLanguage = "batch"; // Removed Scintilla-specific property
                        break;
                }
            }
        }
        
        /// <summary>
        /// Shortcut function to add multiple collection of variable
        /// names at once.
        /// </summary>
        public void SetVariableNames(params string[][] variableNames)
        {
            List<string> varNames = new List<string>();

            foreach (string[] names in variableNames)
            {
                varNames.AddRange(names);
            }

            VariableNames = varNames.ToArray();
        }

        /// <summary>
        /// Loads snippets from the database and populates the context menu.
        /// </summary>
        private void LoadSnippets()
        {
            // Remove existing snippet menu items tagged as "Snippet"
            for (int i = cmnuCommand.Items.Count - 1; i >= 0; i--)
            {
                if (cmnuCommand.Items[i].Tag as string == "Snippet")
                {
                    cmnuCommand.Items.RemoveAt(i);
                }
            }

            // Load snippets from the database
            JsonSnippet[] snippets = JsonDbManager.GetSnippets();
            
            // Add a separator and menu items for each snippet
            if (snippets.Length > 0)
            {
                // Find the position of sepSnippets to insert after it
                int sepIndex = cmnuCommand.Items.IndexOf(sepSnippets);
                int insertIndex = sepIndex + 1;
                
                // Add separator before snippets if not already present
                ToolStripSeparator snippetSeparator = new ToolStripSeparator();
                snippetSeparator.Tag = "Snippet";
                cmnuCommand.Items.Insert(insertIndex, snippetSeparator);
                insertIndex++;

                // Add menu items for each snippet
                foreach (JsonSnippet snippet in snippets)
                {
                    ToolStripMenuItem snippetItem = new ToolStripMenuItem(snippet.Name);
                    snippetItem.Tag = "Snippet";
                    
                    // Create a Snippet object to associate with the menu item
                    Snippet snippetObj = new Snippet()
                    {
                        Guid = !string.IsNullOrEmpty(snippet.SnippetGuid) ? Guid.Parse(snippet.SnippetGuid ?? string.Empty) : Guid.NewGuid(),
                        Name = snippet.Name ?? string.Empty,
                        Type = Command.ConvertToScriptType(snippet.Type ?? string.Empty),
                        Text = snippet.Text ?? string.Empty
                    };
                    
                    // Associate the snippet object with the menu item
                    snippetItem.Tag = snippetObj;
                    
                    // Add event handlers
                    snippetItem.Click += OnInsertSnippetClick;
                    
                    // Add submenu for additional actions
                    ToolStripMenuItem saveAsItem = new ToolStripMenuItem("Save as");
                    saveAsItem.Tag = snippetObj;
                    saveAsItem.Click += OnSaveSnippetAs;
                    snippetItem.DropDownItems.Add(saveAsItem);
                    
                    ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
                    deleteItem.Tag = snippetObj;
                    deleteItem.Click += OnDeleteSnippetClick;
                    snippetItem.DropDownItems.Add(deleteItem);
                    
                    cmnuCommand.Items.Insert(insertIndex, snippetItem);
                    insertIndex++;
                }
            }
        }

        private void bCommand_Click(object sender, EventArgs e)
        {
            cmnuCommand.Show(bCommand, new Point(0, bCommand.Height));
        }

        /// <summary>
        /// Validates the current script (C# only).
        /// </summary>
        private bool ValidateScript(bool confirmOK)
        {
            try
            {
                UserCSScript testInstruction = new UserCSScript(txtCode.Text);

                CompilerErrorCollection errors;
                testInstruction.Compile(out errors);

                // For TextBox, we'll show errors in a simple way
                if (errors.HasErrors)
                {
                    StringBuilder errorText = new StringBuilder();
                    errorText.AppendLine("Script validation errors:");
                    
                    foreach (CompilerError error in errors)
                    {
                        errorText.AppendLine($"Line {error.Line}: {error.ErrorText}");
                    }
                    
                    MessageBox.Show(this, errorText.ToString(), 
                        System.Windows.Forms.Application.ProductName, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }
                else
                {
                    if (confirmOK)
                    {
                        MessageBox.Show(this, "No errors could be found in the script.", 
                            System.Windows.Forms.Application.ProductName, 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "The code cannot be compiled: " + ex.Message, 
                    System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }

            return false;
        }

        #region Command menu

        private void mnuPowerShell_Click(object sender, EventArgs e)
        {
            CommandType = ScriptType.PowerShell;
        }

        private void mnuBatchScript_Click(object sender, EventArgs e)
        {
            CommandType = ScriptType.Batch;
        }

        private void mnuCSScript_Click(object sender, EventArgs e)
        {
            CommandType = ScriptType.CS;
        }

        private void mnuValidate_Click(object sender, EventArgs e)
        {
            ValidateScript(true);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            txtCode.SelectAll();
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            txtCode.Clear();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtCode.Paste();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            txtCode.Copy();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            txtCode.Cut();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            // TextBox doesn't support Redo, so do nothing
        }

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            txtCode.Undo();
        }

        private void mnuRun_Click(object sender, EventArgs e)
        {
            try
            {
                new Command(Text, CommandType).Execute(this.Application!);

                MessageBox.Show(this, "Script executed successfully.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Script can not be executed.\r\n\r\n" + ex.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuNewScript_Click(object sender, EventArgs e)
        {
            using (NewSnippetDialog dialog = new NewSnippetDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string text = string.IsNullOrEmpty(txtCode.SelectedText) ? txtCode.Text : txtCode.SelectedText;
                    Snippet script = new Snippet()
                    {
                        Name = dialog.ScriptName,
                        Text = text,
                        Type = CommandType
                    };
                    script.Save();
                    LoadSnippets();
                }
            }
        }

        private void OnInsertSnippetClick(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                Snippet? snippet = menuItem.Tag as Snippet;
                if (snippet != null)
                {
                    // For TextBox, we'll use SelectedText instead of InsertText
                    txtCode.SelectedText = snippet.Text;
                }
            }
        }

        private void OnDeleteSnippetClick(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                Snippet? snippet = menuItem.Tag as Snippet;
                if (snippet != null)
                {
                    snippet.Delete();
                    LoadSnippets();
                }
            }
        }

        private void OnSaveSnippetAs(object? sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                Snippet? snippet = menuItem.Tag as Snippet;
                if (snippet != null)
                {
                    string text = string.IsNullOrEmpty(txtCode.SelectedText) ? txtCode.Text : txtCode.SelectedText;
                    snippet.Text = text;
                    snippet.Type = CommandType;
                    snippet.Save();
                }
            }
        }

        private void cmnuCommand_Opening(object sender, CancelEventArgs e)
        {
            this.LoadSnippets();

            bool isEditControl = (((ContextMenuStrip)sender).SourceControl == txtCode);
            sepDefaultCommands.Visible = isEditControl;
            mnuCut.Visible = isEditControl;
            mnuCopy.Visible = isEditControl;
            mnuPaste.Visible = isEditControl;
            // TextBox doesn't have CanPaste property, use standard approach
            mnuPaste.Enabled = Clipboard.ContainsText();
            mnuRedo.Visible = isEditControl;
            // TextBox doesn't support Redo, so always disable it
            mnuRedo.Enabled = false;
            mnuUndo.Visible = isEditControl;
            mnuUndo.Enabled = txtCode.CanUndo;
            mnuClear.Visible = isEditControl;
            mnuSelectAll.Visible = isEditControl;
            sepClipboard.Visible = isEditControl;
            sepSelection.Visible = isEditControl;
            
            // Variable menu items should only be visible in edit control too
            foreach (ToolStripItem item in cmnuCommand.Items)
            {
                if (!string.IsNullOrEmpty(item.Tag as string))
                {
                    item.Visible = isEditControl;
                }
            }
        }

        #endregion
    }
}
