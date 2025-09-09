using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CDBurnerXP
{
    /// <summary>
    /// Provides various useful and small functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Determines whether or not a given rectangle is visible
        /// on any of the screens.
        /// </summary>
        public static bool IsRectangleOnAnyScreen(Rectangle rect)
        {
            int minVisibleSize = (int)(rect.Height * rect.Width * 0.15);

            foreach (Screen s in Screen.AllScreens)
            {
                if (rect.IntersectsWith(s.Bounds))
                {
                    Rectangle visibleRect = rect;
                    visibleRect.Intersect(s.Bounds);
                    if (!visibleRect.IsEmpty)
                    {
                        // 15% sichtbar?
                        int visibleSize = visibleRect.Height * visibleRect.Width;
                        if (visibleSize >= minVisibleSize)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Compacts a path string withh en ellipsis if necessary.
        /// </summary>
        /// <param name="myString">Path to compact</param>
        /// <param name="width">Width in px available for the string</param>
        /// <param name="font">Font used to render the string</param>
        /// <param name="otherText">Reduce the available by the width of this text</param>
        public static string CompactString(string myString, int width, Font font, string otherText)
        {
            string result = new string(myString); // Replace string.Copy with new string constructor
            width -= TextRenderer.MeasureText(otherText, font).Width;
            
            // Replace TextFormatFlags.ModifyString with a safer approach
            Size textSize = TextRenderer.MeasureText(result, font, new Size(width, 0), TextFormatFlags.PathEllipsis);
            if (TextRenderer.MeasureText(result, font).Width > width)
            {
                // Manually truncate and add ellipsis
                result = TruncateWithEllipsis(result, width, font);
            }
            return result;
        }
        
        /// <summary>
        /// Truncates a string and adds ellipsis (...) to fit within a specified width
        /// </summary>
        private static string TruncateWithEllipsis(string text, int maxWidth, Font font)
        {
            if (string.IsNullOrEmpty(text)) return text;
            
            string ellipsis = "...";
            Size ellipsisSize = TextRenderer.MeasureText(ellipsis, font);
            
            if (TextRenderer.MeasureText(text, font).Width <= maxWidth)
                return text;
                
            int maxTextWidth = maxWidth - ellipsisSize.Width;
            if (maxTextWidth <= 0) return ellipsis;
            
            string truncated = text;
            while (TextRenderer.MeasureText(truncated, font).Width > maxTextWidth && truncated.Length > 0)
            {
                truncated = truncated.Substring(0, truncated.Length - 1);
            }
            
            return truncated + ellipsis;
        }
        
        /// <summary>
        /// Converts a string to its url encoded equivalent. Encodes more chars than necessary, but that won't hurt.
        /// </summary>
        /// <remarks>.NET native function: System.Web.HttpUtility.UrlEncode, requires Sytem.Web.dll reference</remarks>
        public static string UrlEncode(string str)
        {
            StringBuilder newString = new StringBuilder();
            ASCIIEncoding enc = new ASCIIEncoding();

            byte[] chars = enc.GetBytes(str);
            return UrlEncode(chars);
        }

        /// <summary>
        /// Converts a byte array to its url encoded equivalent. Encodes more chars than necessary, but that won't hurt.
        /// </summary>
        /// <remarks>.NET native function: System.Web.HttpUtility.UrlEncode, requires Sytem.Web.dll reference</remarks>
        public static string UrlEncode(byte[] bytes)
        {
            StringBuilder newString = new StringBuilder();
            
            foreach (byte part in bytes) {
                newString.Append("%" + ((int)part).ToString("X2"));
            }
            
            return newString.ToString();
        }

        /// <summary>
        /// Reduces a TimeSpan duration to seconds (discarding miliseconds).
        /// </summary>
        public static TimeSpan TimeSpanSeconds(TimeSpan value)
        {
            if (value.TotalSeconds < 0)
                return TimeSpan.FromSeconds(0);
            try
            {
                return TimeSpan.FromSeconds((int)(value).TotalSeconds);
            }
            catch (OverflowException)
            {
                // such problems may occur if the user changes his clock settings or the like
                // during burning process
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Converts a given number of bytes to a char-array and then to a string.
        /// </summary>
        public static string ConvertBytesToString(byte[] bytes)
        {
            return ConvertBytesToString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Converts a given number of bytes to a char-array and then to a string.
        /// </summary>
        public static string ConvertBytesToString(byte[] bytes, int offset, int length)
        {
            char[] chars = new char[length];
            for (int i = offset; i < offset + length; i++)
            {
                chars[i] = (char)bytes[i];
            }
            return new String(chars);
        }

        /// <summary>
        /// Converts a number of bytes to long integer.
        /// Each byte is interpreted as two hex digits.
        /// </summary>
        public static long ConvertBytesToInt64(byte[] bytes, int offset)
        {
            string hexNum = string.Empty;
            for (int i = bytes.Length - 1; i >= offset; i--)
            {
                hexNum = bytes[i].ToString("X2") + hexNum;
            }
            return Convert.ToInt64(hexNum, 16);
        }
    }
}
