using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// A BarRenderer draws a bar in a cell
    /// </summary>
    public class BarRenderer : BaseRenderer
    {
        /// <summary>
        /// Make a quiet renderer
        /// </summary>
        public BarRenderer()
            : base()
        {
        }

        /// <summary>
        /// Make a bar renderer for the given range of values
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public BarRenderer(int minimum, int maximum)
            : this()
        {
            this.MinimumValue = minimum;
            this.MaximumValue = maximum;
        }

        /// <summary>
        /// Make a bar renderer using a custom pen and brush
        /// </summary>
        /// <param name="aPen"></param>
        /// <param name="aBrush"></param>
        public BarRenderer(Pen aPen, Brush aBrush)
            : this()
        {
            this.Pen = aPen;
            this.Brush = aBrush;
        }

        /// <summary>
        /// Make a bar renderer using a custom pen and brush for the given range of values
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="aPen"></param>
        /// <param name="aBrush"></param>
        public BarRenderer(int minimum, int maximum, Pen aPen, Brush aBrush)
            : this(minimum, maximum)
        {
            this.Pen = aPen;
            this.Brush = aBrush;
        }

        /// <summary>
        /// Make a bar renderer using a gradient fill
        /// </summary>
        /// <param name="aPen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public BarRenderer(Pen aPen, Color start, Color end)
            : this()
        {
            this.Pen = aPen;
            this.SetGradient(start, end);
        }

        /// <summary>
        /// Make a bar renderer using a gradient fill for the given range of values
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="aPen"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public BarRenderer(int minimum, int maximum, Pen aPen, Color start, Color end)
            : this(minimum, maximum)
        {
            this.Pen = aPen;
            this.SetGradient(start, end);
        }

        #region Configuration Properties

        /// <summary>
        /// Should the bar be drawn using a standard bar appearance?
        /// </summary>
        public bool UseStandardBar = true;

        /// <summary>
        /// How many pixels should be left between the bar and the edge of the cell?
        /// </summary>
        public int Padding = 2;

        /// <summary>
        /// The pen used to draw the bar border
        /// </summary>
        public Pen? Pen;

        /// <summary>
        /// The brush used to fill the bar
        /// </summary>
        public Brush? Brush;

        /// <summary>
        /// The brush used to fill the background of the bar
        /// </summary>
        public Brush? BackgroundBrush;

        /// <summary>
        /// The start color of the gradient
        /// </summary>
        public Color StartColor;

        /// <summary>
        /// The end color of the gradient
        /// </summary>
        public Color EndColor;

        /// <summary>
        /// The maximum width of the bar
        /// </summary>
        public int MaximumWidth = 100;

        /// <summary>
        /// The maximum height of the bar
        /// </summary>
        public int MaximumHeight = 16;

        /// <summary>
        /// The minimum value for the bar
        /// </summary>
        public int MinimumValue = 0;

        /// <summary>
        /// The maximum value for the bar
        /// </summary>
        public int MaximumValue = 100;

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the gradient colors for the bar
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SetGradient(Color start, Color end)
        {
            this.StartColor = start;
            this.EndColor = end;
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Draw our data value
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r)
        {
            this.DrawBackground(g, r);

            if (this.UseStandardBar)
                this.DrawStandardBar(g, r);
            else
                this.DrawGradientBar(g, r);
        }

        /// <summary>
        /// Draw a standard bar
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected void DrawStandardBar(Graphics g, Rectangle r)
        {
            Rectangle barBounds = this.CalculateBarBounds(r);

            // Draw the background
            if (this.BackgroundBrush != null)
                g.FillRectangle(this.BackgroundBrush, barBounds);

            // Draw the bar
            Rectangle fillRect = new Rectangle(barBounds.X, barBounds.Y,
                (int)(barBounds.Width * this.GetProportion()), barBounds.Height);

            if (this.Brush != null)
                g.FillRectangle(this.Brush, fillRect);

            // Draw the border
            if (this.Pen != null)
                g.DrawRectangle(this.Pen, barBounds);
        }

        /// <summary>
        /// Draw a gradient bar
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected void DrawGradientBar(Graphics g, Rectangle r)
        {
            Rectangle barBounds = this.CalculateBarBounds(r);

            // Draw the background
            if (this.BackgroundBrush != null)
                g.FillRectangle(this.BackgroundBrush, barBounds);

            // Draw the gradient bar
            Rectangle fillRect = new Rectangle(barBounds.X, barBounds.Y,
                (int)(barBounds.Width * this.GetProportion()), barBounds.Height);

            if (!this.StartColor.IsEmpty && !this.EndColor.IsEmpty)
            {
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    fillRect, this.StartColor, this.EndColor, LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(gradientBrush, fillRect);
                }
            }

            // Draw the border
            if (this.Pen != null)
                g.DrawRectangle(this.Pen, barBounds);
        }

        /// <summary>
        /// Calculate the bounds of the bar
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        protected Rectangle CalculateBarBounds(Rectangle r)
        {
            Rectangle barBounds = new Rectangle(r.X + this.Padding, r.Y + this.Padding,
                Math.Min(r.Width - (this.Padding * 2), this.MaximumWidth),
                Math.Min(r.Height - (this.Padding * 2), this.MaximumHeight));

            // Center the bar vertically
            barBounds.Y = r.Y + (r.Height - barBounds.Height) / 2;

            return barBounds;
        }

        /// <summary>
        /// Get the proportion of the bar to fill
        /// </summary>
        /// <returns></returns>
        protected float GetProportion()
        {
            if (!(this.Aspect is IConvertible))
                return 0.0f;

            double aspectValue = ((IConvertible)this.Aspect).ToDouble(null);
            double range = this.MaximumValue - this.MinimumValue;

            if (range <= 0)
                return 0.0f;

            double proportion = (aspectValue - this.MinimumValue) / range;
            return (float)Math.Max(0.0, Math.Min(1.0, proportion));
        }

        #endregion
    }
}