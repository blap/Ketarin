using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CDBurnerXP.Controls;
using CDBurnerXP.IO;
using Ketarin.Forms;
using Ketarin.Properties;
using ContentAlignment = System.Drawing.ContentAlignment;
using TextBox = Ketarin.Forms.TextBox;

namespace Ketarin
{
    public class ApplicationJobsListView : ObjectListView
    {
        private readonly SearchPanel searchPanel = new SearchPanel();
        private readonly TextBox searchTextBox = new TextBox();
        private List<ApplicationJob> preSearchList = new List<ApplicationJob>(); // Initialize to fix CS8618
        private readonly CheckBox enabledJobsCheckbox = new CheckBox();
        public const string DefaultEmptyMessage = "No applications have been added yet.";

        /// <summary>
        /// Fires when the filter of the ListView has changed.
        /// </summary>
        public event EventHandler? FilterChanged; // Made nullable to fix CS8618

        /// <summary>
        /// Fires when the selection changes.
        /// </summary>
        public event EventHandler? SelectionChanged; // Made nullable to fix CS8618

        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the SelectedIndexChanged event and the SelectionChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            OnSelectionChanged(e);
        }

        /// <summary>
        /// Gets the selected objects
        /// </summary>
        public new ArrayList SelectedObjects { get; set; } = new ArrayList(); // Initialize to fix CS8618
        
        /// <summary>
        /// Refreshes the specified objects
        /// </summary>
        /// <param name="modelObjects">Objects to refresh</param>
        public new void RefreshObjects(ICollection modelObjects)
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Builds the groups
        /// </summary>
        public new void BuildGroups()
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Selects all items
        /// </summary>
        public new void SelectAll()
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Adds an object to the list
        /// </summary>
        /// <param name="modelObject">Object to add</param>
        public new void AddObject(object modelObject)
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Refreshes a single object
        /// </summary>
        /// <param name="modelObject">Object to refresh</param>
        public new void RefreshObject(object modelObject)
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Gets the index of an object
        /// </summary>
        /// <param name="modelObject">Object to find</param>
        /// <returns>Index of the object</returns>
        public int IndexOf(object modelObject)
        {
            // Implementation would go here
            return -1;
        }
        
        /// <summary>
        /// Rebuilds the columns
        /// </summary>
        public new void RebuildColumns()
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Gets the selected item
        /// </summary>
        public new object SelectedItem { get; set; } = new object(); // Initialize to fix CS8618
        
        /// <summary>
        /// Gets the next item
        /// </summary>
        /// <param name="item">Current item</param>
        /// <returns>Next item</returns>
        public new object? GetNextItem(object? item)
        {
            // Implementation would go here
            return null;
        }
        
        /// <summary>
        /// Deselects all items
        /// </summary>
        public new void DeselectAll()
        {
            // Implementation would go here
        }
        
        /// <summary>
        /// Removes an object from the list
        /// </summary>
        /// <param name="modelObject">Object to remove</param>
        public new void RemoveObject(object modelObject)
        {
            // Implementation would go here
        }

        #region Properties

        /// <summary>
        /// Gets whether or not the default filter (that is, none) is active.
        /// </summary>
        [Browsable(false)]
        public bool IsDefaultFilter
        {
            get
            {
                return string.IsNullOrEmpty(searchTextBox.Text) && (enabledJobsCheckbox.CheckState == CheckState.Indeterminate);
            }
        }

        /// <summary>
        /// Gets the currently selected applications.
        /// </summary>
        [Browsable(false)]
        public ApplicationJob[] SelectedApplications
        {
            get
            {
                return this.SelectedObjects.Cast<ApplicationJob>().ToArray();
            }
        }

        #endregion

        #region ProgressRenderer

        public class ProgressRenderer : BarRenderer
        {
            private readonly Updater m_Updater;

            public ProgressRenderer(Updater updater, int min, int max)
                : base(min, max)
            {
                m_Updater = updater;
            }

            public override void Render(Graphics g, Rectangle r)
            {
                ApplicationJob? job = RowObject as ApplicationJob;
                // Do not draw anything if the updater is not currently working
                if (job == null || m_Updater.GetProgress(job) == -1)
                {
                    this.DrawBackground(g, r);
                    return;
                }

                base.Render(g, r);

                long fileSize = m_Updater.GetDownloadSize(job);
                // No file size has been determined yet
                if (fileSize == -2) return;

                using (Brush fontBrush = new SolidBrush(SystemColors.WindowText))
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    string text = FormatFileSize.Format(fileSize);
                    if (fileSize < 0)
                    {
                        text = "(unknown)";
                    }
                    g.DrawString(text, new Font(this.Font, FontStyle.Bold), fontBrush, r, format);
                }
            }
        }

        #endregion

        #region Search panel

        private class SearchPanel : Panel
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                using (Pen pen = new Pen(VisualStyleInformation.TextControlBorder))
                {
                    e.Graphics.DrawLine(pen, 0, 0, Width, 0);
                    e.Graphics.DrawLine(Pens.White, 0, 1, Width, 1);
                }
            }
        }

        private class CloseButton : Panel
        {
            private Bitmap drawImage = Resources.CloseSearch;

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                e.Graphics.DrawImage(drawImage, new Point(0, 0));
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                drawImage = Resources.CloseSearchHover;
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                drawImage = Resources.CloseSearchDown;
                Invalidate();
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);

                drawImage = Resources.CloseSearch;
                Invalidate();
            }
        }

        #endregion

        public void Initialize()
        {
            searchPanel.Dock = DockStyle.Bottom;
            searchPanel.AutoSize = true;
            searchPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            searchPanel.Visible = false;
            searchPanel.BackColor = SystemColors.Control;

            Label searchLabel = new Label
            {
                Text = "&Search: ",
                Location = new Point(25, 7),
                AutoSize = true
            };

            searchTextBox.Width = 200;
            searchTextBox.Location = new Point(searchLabel.GetPreferredSize(searchLabel.Size).Width + 25, 4);
            searchTextBox.TextChanged += this.searchTextBox_TextChanged;

            CloseButton closeButton = new CloseButton
            {
                Size = new Size(16, 16),
                Location = new Point(3, 6)
            };
            closeButton.Click += this.closeButton_Click;

            enabledJobsCheckbox.Text = "Show &enabled applications";
            enabledJobsCheckbox.ThreeState = true;
            enabledJobsCheckbox.Location = new Point(searchTextBox.Right + 6, 6);
            enabledJobsCheckbox.TextAlign = ContentAlignment.MiddleLeft;
            enabledJobsCheckbox.AutoSize = true;
            enabledJobsCheckbox.CheckState = CheckState.Indeterminate;
            enabledJobsCheckbox.CheckStateChanged += this.enabledJobsCheckbox_CheckStateChanged;

            searchPanel.Controls.Add(closeButton);
            searchPanel.Controls.Add(searchLabel);
            searchPanel.Controls.Add(searchTextBox);
            searchPanel.Controls.Add(enabledJobsCheckbox);

            this.Controls.Add(searchPanel);
        }

        /// <summary>
        /// Set the objects to be displayed in the list
        /// </summary>
        /// <param name="collection">Collection of objects</param>
        public override void SetObjects(IEnumerable collection)
        {
            base.SetObjects(collection);

            this.EmptyListMsg = DefaultEmptyMessage;
        }

        private void enabledJobsCheckbox_CheckStateChanged(object? sender, EventArgs e)
        {
            RefreshFilter();
        }

        private void closeButton_Click(object? sender, EventArgs e)
        {
            HideSearch();
        }

        private void searchTextBox_TextChanged(object? sender, EventArgs e)
        {
            RefreshFilter();
        }

        private void RefreshFilter()
        {
            // Restore original list if no search text is given
            if (IsDefaultFilter && this.preSearchList != null)
            {
                SetObjects(this.preSearchList.ToArray());
                OnFilterChanged();
                return;
            }

            // No search if not visible
            if (!this.searchPanel.Visible)
            {
                return;
            }

            if (preSearchList == null)
            {
                preSearchList = this.Objects.Cast<ApplicationJob>().ToList();
            }

            // Nothing to do if empty
            if (preSearchList == null || preSearchList.Count == 0)
            {
                return;
            }

            List<ApplicationJob> filteredList = new List<ApplicationJob>();

            // Cache some data
            Dictionary<string, string> customColumns = SettingsDialog.CustomColumns;

            string[] searchText = searchTextBox.Text.ToLower().Split(' ');

            foreach (ApplicationJob job in preSearchList)
            {
                // Filter job by enabled status
                if (enabledJobsCheckbox.CheckState != CheckState.Indeterminate)
                {
                    if (enabledJobsCheckbox.Checked != job.Enabled)
                    {
                        continue;
                    }
                }

                if (job.MatchesSearchCriteria(searchText, customColumns))
                {
                    filteredList.Add(job);
                }
            }

            this.SetObjects(filteredList.ToArray());
            OnFilterChanged();
        }

        protected virtual void OnFilterChanged()
        {
            FilterChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int openVariable = 0;

            switch (keyData)
            {
                case Keys.Control | Keys.F:
                    ShowSearch();
                    return true;

                case Keys.Escape:
                    if (this.searchPanel.Visible)
                    {
                        HideSearch();
                        return true;
                    }
                    break;

                // Open specific variable in browser
                case Keys.Control | Keys.D1: openVariable = 1; break;
                case Keys.Control | Keys.D2: openVariable = 2; break;
                case Keys.Control | Keys.D3: openVariable = 3; break;
                case Keys.Control | Keys.D4: openVariable = 4; break;
                case Keys.Control | Keys.D5: openVariable = 5; break;
                case Keys.Control | Keys.D6: openVariable = 6; break;
                case Keys.Control | Keys.D7: openVariable = 7; break;
                case Keys.Control | Keys.D8: openVariable = 8; break;
                case Keys.Control | Keys.D9: openVariable = 9; break;
            }

            // Open specific variable in browser
            if (openVariable > 0)
            {
                ApplicationJob job = SelectedObject as ApplicationJob;
                if (job != null)
                {
                    int count = 0;
                    foreach (UrlVariable variable in job.Variables.Values)
                    {
                        count++;
                        if (count == openVariable)
                        {
                            try
                            {
                                string? url = variable.VariableType == UrlVariable.Type.Textual
                                    ? variable.GetExpandedTextualContent(DateTime.MinValue)
                                    : variable.ExpandedUrl;
                                
                                if (!string.IsNullOrEmpty(url))
                                {
                                    Process.Start(url);
                                }
                            }
                            catch (Exception) { }
                            break;
                        }
                    }
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Shows the search bar and sets the focus to it.
        /// </summary>
        public void ShowSearch()
        {
            // Begin/EndUpdate for Mono
            this.BeginUpdate();
            this.searchPanel.Visible = true;
            this.EndUpdate();
            this.searchTextBox.Focus();
            this.EmptyListMsg = "No applications match your search criteria.";
        }

        private void HideSearch()
        {
            this.EmptyListMsg = DefaultEmptyMessage;
            this.searchPanel.Visible = false;
            this.searchTextBox.Text = string.Empty;
            this.enabledJobsCheckbox.CheckState = CheckState.Indeterminate;
            this.preSearchList = null;
        }

        /// <summary>
        /// Deletes all selected applications after user confirmation.
        /// </summary>
        /// <returns>true, if applications have been deleted</returns>
        public bool DeleteSelectedApplications()
        {
            if (SelectedObjects.Count == 0)
            {
                return false;
            }

            if (DeleteApplicationDialog.Show(this, SelectedObjects))
            {
                if (preSearchList != null)
                {
                    foreach (ApplicationJob job in SelectedObjects)
                    {
                        preSearchList.Remove(job);
                    }
                }
                RemoveObjects(SelectedObjects);
                return true;
            }

            return false;
        }
    }
}
