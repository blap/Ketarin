using System;
using System.Drawing;
using System.Windows.Forms;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// Interface for rendering cells in an ObjectListView
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Render the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        void Render(Graphics g, Rectangle r);
        
        /// <summary>
        /// Draw the background of the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        void DrawBackground(Graphics g, Rectangle r);
        
        /// <summary>
        /// Draw the text of the cell
        /// </summary>
        /// <param name="g">Graphics context</param>
        /// <param name="r">Rectangle bounds</param>
        void DrawText(Graphics g, Rectangle r);
        
        /// <summary>
        /// Get the text to be displayed
        /// </summary>
        /// <returns>The text to display</returns>
        string GetText();
        
        /// <summary>
        /// Get the image to be displayed
        /// </summary>
        /// <returns>The image to display</returns>
        Image? GetImage();
    }
}