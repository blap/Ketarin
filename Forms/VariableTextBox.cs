using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Ketarin.Forms
{
    /// <summary>
    /// A usual textbox plus variable selection tool.
    /// </summary>
    internal class VariableTextBox : TextBox
    {
        private string[] m_VariableNames = new string[0];
        private ContextMenuStrip? m_Customiser;
        private bool m_EnableEditor = true;

        #region Properties

        /// <summary>
        /// Gets or sets the variable names to show
        /// within the context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string[] VariableNames
        {
            get { return m_VariableNames; }
            set {
                if (m_VariableNames != value)
                {
                    m_VariableNames = value;
                    RebuildContextMenu();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not an editor for the 
        /// text box can be opened from the context menu.
        /// </summary>
        [DefaultValue(true)]
        public bool EnableEditor
        {
            get { return m_EnableEditor; }
            set { m_EnableEditor = value; }
        }

        #endregion

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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            m_Customiser = new ContextMenuStrip();
            this.ContextMenuStrip = m_Customiser;
            RebuildContextMenu();
        }

        /// <summary>
        /// Adds all context menu items based on the given variable names.
        /// </summary>
        private void RebuildContextMenu()
        {
            if (m_Customiser == null) return;

            m_Customiser.Items.Clear();

            // No options for read only text boxes
            if (ReadOnly) return;

            if (m_VariableNames.Length > 0)
            {

                List<string> vars = new List<string>(m_VariableNames);
                vars.Sort();
                // We insert in reverse order
                vars.Reverse();

                // Add final separator
                m_Customiser.Items.Add(new ToolStripSeparator());

                foreach (string var in vars)
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem("{" + var + "}");
                    newItem.Click += (sender, e) => OnVariableSelected(newItem.Text);
                    m_Customiser.Items.Add(newItem);
                }
            }

            // Add context menu item for multiline editor
            if (Multiline && m_EnableEditor)
            {
                // Add final separator
                m_Customiser.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem newItem = new ToolStripMenuItem("Editor...");
                newItem.Click += (sender, e) =>
                {
                    using (MultilineEditorDialog dialog = new MultilineEditorDialog())
                    {
                        dialog.Value = this.Text;
                        dialog.SetVariableNames(m_VariableNames);

                        if (dialog.ShowDialog(this) == DialogResult.OK)
                        {
                            this.Text = dialog.Value;
                        }
                    }
                };
                m_Customiser.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Inserts the selected variable at the current cursor position.
        /// </summary>
        private void OnVariableSelected(string menuItemText)
        {
            int selStart = SelectionStart;
            Text = Text.Insert(selStart, menuItemText);
            SelectionStart = selStart + menuItemText.Length;
        }
    }
}
