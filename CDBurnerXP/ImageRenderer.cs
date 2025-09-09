using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// An ImageRenderer draws an image in a cell
    /// </summary>
    public class ImageRenderer : BaseRenderer
    {
        /// <summary>
        /// Create an ImageRenderer
        /// </summary>
        public ImageRenderer()
            : base()
        {
        }

        /// <summary>
        /// Create an ImageRenderer that starts animations
        /// </summary>
        /// <param name="startAnimations"></param>
        public ImageRenderer(bool startAnimations)
            : this()
        {
            this.startAnimations = startAnimations;
        }

        #region Public Properties

        /// <summary>
        /// Should animations be started when the renderer is created?
        /// </summary>
        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }
        private bool paused = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Pause the animation
        /// </summary>
        public void Pause()
        {
            this.paused = true;
        }

        /// <summary>
        /// Unpause the animation
        /// </summary>
        public void Unpause()
        {
            this.paused = false;
        }

        /// <summary>
        /// Handle timer events
        /// </summary>
        /// <param name="state"></param>
        public void OnTimer(Object state)
        {
            // Implementation would go here
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Draw our image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public override void Render(Graphics g, Rectangle r)
        {
            this.DrawBackground(g, r);

            Image? image = this.GetImage();
            if (image == null)
                return;

            // Calculate where the image should be drawn
            Rectangle imageBounds = r;
            imageBounds.Width = image.Width;
            imageBounds.Height = image.Height;
            imageBounds = this.AlignRectangle(r, imageBounds);

            // Draw the image
            g.DrawImage(image, imageBounds);
        }

        #endregion

        #region Animation Support

        /// <summary>
        /// Instances of this class kept track of the animation state of a single image.
        /// </summary>
        internal class AnimationState
        {
            /// <summary>
            /// Is the given image an animation?
            /// </summary>
            /// <param name="image"></param>
            /// <returns></returns>
            static public bool IsAnimation(Image image)
            {
                if (image == null)
                    return false;

                // Check if the image has multiple frames
                Guid[] dimensions = image.FrameDimensionsList;
                foreach (Guid guid in dimensions)
                {
                    FrameDimension dimension = new FrameDimension(guid);
                    if (dimension.Equals(FrameDimension.Time))
                    {
                        return image.GetFrameCount(FrameDimension.Time) > 1;
                    }
                }

                return false;
            }

            /// <summary>
            /// Create an AnimationState
            /// </summary>
            public AnimationState()
            {
            }

            /// <summary>
            /// Create an AnimationState for the given image
            /// </summary>
            /// <param name="image"></param>
            public AnimationState(Image image)
            {
                this.image = image;
                this.IsValid = IsAnimation(image);
            }

            #region Public Properties

            /// <summary>
            /// Is this animation valid?
            /// </summary>
            public bool IsValid
            {
                get { return isValid; }
                set { isValid = value; }
            }
            private bool isValid = false;

            #endregion

            #region Public Methods

            /// <summary>
            /// Advance the frame
            /// </summary>
            /// <param name="millisecondsNow"></param>
            public void AdvanceFrame(long millisecondsNow)
            {
                // Implementation would go here
            }

            #endregion

            private Image? image;
        }

        private bool startAnimations = true;

        #endregion
    }
}