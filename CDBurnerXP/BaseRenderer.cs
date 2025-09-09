using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// A BaseRenderer is the base class for rendering cells in an ObjectListView.
    /// </summary>
    public class BaseRenderer : IRenderer
    {
        /// <summary>
        /// Create a BaseRenderer
        /// </summary>
        public BaseRenderer()
        {
        }

        #region IRenderer Implementation

        /// <summary>
        /// Render the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        public virtual void Render(Graphics g, Rectangle r)
        {
            this.DrawBackground(g, r);
            this.DrawText(g, r);
        }
        
        /// <summary>
        /// Draw the background of the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        public virtual void DrawBackground(Graphics g, Rectangle r)
        {
            if (!this.IsDrawBackground)
                return;

            Color backgroundColor = this.GetBackgroundColor();
            if (backgroundColor.IsEmpty)
                return;

            using (Brush brush = new SolidBrush(backgroundColor))
            {
                g.FillRectangle(brush, r);
            }
        }
        
        /// <summary>
        /// Draw the text of the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        public virtual void DrawText(Graphics g, Rectangle r)
        {
            string text = this.GetText();
            if (String.IsNullOrEmpty(text))
                return;

            Font font = this.Font ?? (this.ListView != null ? this.ListView.Font : Control.DefaultFont);
            Brush brush = this.TextBrush ?? Brushes.Black;

            // Handle text alignment
            StringFormat format = new StringFormat();
            if (this.Column != null)
            {
                switch (this.Column.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        format.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        format.Alignment = StringAlignment.Far;
                        break;
                    default:
                        format.Alignment = StringAlignment.Near;
                        break;
                }
            }

            if (this.CanWrap)
            {
                format.FormatFlags = StringFormatFlags.LineLimit;
                g.DrawString(text, font, brush, r, format);
            }
            else
            {
                format.Trimming = StringTrimming.EllipsisCharacter;
                g.DrawString(text, font, brush, r, format);
            }
        }
        
        /// <summary>
        /// Get the text to be displayed
        /// </summary>
        /// <returns>The text to display</returns>
        public virtual string GetText()
        {
            if (this.Aspect == null)
                return string.Empty;

            return this.Aspect.ToString() ?? string.Empty;
        }
        
        /// <summary>
        /// Get the image to be displayed
        /// </summary>
        /// <returns>The image to display</returns>
        public virtual Image? GetImage()
        {
            if (this.Column == null)
                return null;

            return this.GetImageFromSelector(this.Column.GetImage(this.RowObject ?? new object()));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or set the OLVColumn that this renderer will draw
        /// </summary>
        public OLVColumn? Column
        {
            get { return column; }
            set { column = value; }
        }
        private OLVColumn? column;

        /// <summary>
        /// Get or set the DrawListViewSubItemEventArgs that this renderer will draw
        /// </summary>
        public DrawListViewSubItemEventArgs? Event
        {
            get { return eventArgs; }
            set { eventArgs = value; }
        }
        private DrawListViewSubItemEventArgs? eventArgs;

        /// <summary>
        /// Get or set the DrawListViewItemEventArgs that this renderer will draw
        /// </summary>
        public DrawListViewItemEventArgs? DrawItemEvent
        {
            get { return drawItemEventArgs; }
            set { drawItemEventArgs = value; }
        }
        private DrawListViewItemEventArgs? drawItemEventArgs;

        /// <summary>
        /// Get or set the ObjectListView that this renderer will draw
        /// </summary>
        public ObjectListView? ListView
        {
            get { return objectListView; }
            set { objectListView = value; }
        }
        private ObjectListView? objectListView;

        /// <summary>
        /// Get or set the row object that this renderer will draw
        /// </summary>
        public Object? RowObject
        {
            get { return rowObject; }
            set { rowObject = value; }
        }
        private Object? rowObject;

        /// <summary>
        /// Get or set the aspect of the row object that this renderer will draw
        /// </summary>
        public Object? Aspect
        {
            get { return aspect; }
            set { aspect = value; }
        }
        private Object? aspect;

        /// <summary>
        /// Get or set the OLVListItem that this renderer will draw
        /// </summary>
        public OLVListItem? ListItem
        {
            get { return listItem; }
            set { listItem = value; }
        }
        private OLVListItem? listItem;

        /// <summary>
        /// Get or set the ListViewItem.ListViewSubItem that this renderer will draw
        /// </summary>
        public ListViewItem.ListViewSubItem? SubItem
        {
            get { return subItem; }
            set { subItem = value; }
        }
        private ListViewItem.ListViewSubItem? subItem;

        /// <summary>
        /// Get or set the OLVListSubItem that this renderer will draw
        /// </summary>
        public OLVListSubItem? OLVSubItem
        {
            get { return olvSubItem; }
            set { olvSubItem = value; }
        }
        private OLVListSubItem? olvSubItem;

        /// <summary>
        /// Get or set whether the item is selected
        /// </summary>
        public bool IsItemSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        private bool isSelected;

        /// <summary>
        /// Get or set the font to be used for text
        /// </summary>
        public Font? Font
        {
            get { return font; }
            set { font = value; }
        }
        private Font? font;

        /// <summary>
        /// Get or set the brush to be used for text
        /// </summary>
        public Brush? TextBrush
        {
            get { return textBrush; }
            set { textBrush = value; }
        }
        private Brush? textBrush;

        /// <summary>
        /// Get or set whether to draw the background
        /// </summary>
        public bool IsDrawBackground
        {
            get { return isDrawBackground; }
            set { isDrawBackground = value; }
        }
        private bool isDrawBackground = true;

        /// <summary>
        /// Get or set whether text can wrap
        /// </summary>
        public bool CanWrap
        {
            get { return canWrap; }
            set { canWrap = value; }
        }
        private bool canWrap = false;

        /// <summary>
        /// Get or set the spacing between elements
        /// </summary>
        public int Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }
        private int spacing = 2;

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the background color to be used
        /// </summary>
        /// <returns>The background color</returns>
        public Color GetBackgroundColor()
        {
            if (this.IsItemSelected && this.ListView != null)
                return this.ListView.HighlightBackgroundColorOrDefault;
            else
                return Color.Empty;
        }

        /// <summary>
        /// Handle rendering of the cell
        /// </summary>
        /// <param name="e"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="rowObject"></param>
        /// <returns></returns>
        public bool HandleRendering(EventArgs e, Graphics g, Rectangle r, Object rowObject)
        {
            // Implementation would go here
            return true;
        }

        /// <summary>
        /// Optionally render the cell
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        virtual public bool OptionalRender(Graphics g, Rectangle r)
        {
            this.Render(g, r);
            return true;
        }

        /// <summary>
        /// Align a rectangle within another rectangle
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        public Rectangle AlignRectangle(Rectangle outer, Rectangle inner)
        {
            Rectangle r = new Rectangle(outer.Location, inner.Size);

            // Align horizontally
            switch (this.Column != null ? this.Column.TextAlign : HorizontalAlignment.Left)
            {
                case HorizontalAlignment.Center:
                    r.X = outer.X + (outer.Width - inner.Width) / 2;
                    break;
                case HorizontalAlignment.Right:
                    r.X = outer.Right - inner.Width;
                    break;
            }

            // Align vertically
            r.Y = outer.Y + (outer.Height - inner.Height) / 2;

            return r;
        }

        /// <summary>
        /// Get the image from an image selector
        /// </summary>
        /// <param name="imageSelector">The image selector</param>
        /// <returns>The image to display</returns>
        public Image? GetImageFromSelector(Object? imageSelector)
        {
            if (imageSelector == null)
                return null;

            if (imageSelector is Image)
                return (Image)imageSelector;

            if (this.ListView == null || this.ListView.SmallImageList == null)
                return null;

            if (imageSelector is Int32)
            {
                Int32 index = (Int32)imageSelector;
                if (index >= 0 && index < this.ListView.SmallImageList.Images.Count)
                    return this.ListView.SmallImageList.Images[index];
            }
            else if (imageSelector is String)
            {
                String key = (String)imageSelector;
                if (this.ListView.SmallImageList.Images.ContainsKey(key))
                    return this.ListView.SmallImageList.Images[key];
            }

            return null;
        }

        #endregion
    }
}