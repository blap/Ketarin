using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Ketarin
{
    /// <summary>
    /// Provides menu compatibility methods for .NET 6 migration.
    /// Handles deprecated menu APIs and provides modern alternatives.
    /// </summary>
    internal static class MenuCompatibility
    {
        #region WinAPI Declarations

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool IsMenu(IntPtr hMenu);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MENUITEMINFO
        {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;
        }

        private const uint MN_GETHMENU = 0x01E1;
        private const uint WM_INITMENUPOPUP = 0x0117;
        private const uint WM_MENUSELECT = 0x011F;
        private const uint WM_COMMAND = 0x0111;

        #endregion

        #region Menu Compatibility Methods

        /// <summary>
        /// Safely finds a window handle with error handling for .NET 6.
        /// </summary>
        public static IntPtr SafeFindWindow(string className, string windowName)
        {
            try
            {
                return FindWindow(className, windowName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error finding window: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Safely checks if a handle is a valid menu.
        /// </summary>
        public static bool SafeIsMenu(IntPtr hMenu)
        {
            if (hMenu == IntPtr.Zero)
                return false;

            try
            {
                return IsMenu(hMenu);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking menu handle: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the menu handle from a window with proper error handling.
        /// </summary>
        public static IntPtr GetMenuHandle(IntPtr windowHandle)
        {
            try
            {
                return SendMessage(windowHandle, MN_GETHMENU, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting menu handle: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Creates a modern context menu strip as an alternative to native menus.
        /// </summary>
        public static ContextMenuStrip CreateModernContextMenu(IEnumerable<ToolStripItem> items)
        {
            var menu = new ContextMenuStrip();

            foreach (var item in items)
            {
                menu.Items.Add(item);
            }

            return menu;
        }

        /// <summary>
        /// Adds a separator to a context menu with compatibility.
        /// </summary>
        public static void AddSeparator(ContextMenuStrip menu)
        {
            if (menu != null)
            {
                menu.Items.Add(new ToolStripSeparator());
            }
        }

        /// <summary>
        /// Safely removes a menu item by index.
        /// </summary>
        public static bool SafeRemoveMenuItem(IntPtr hMenu, uint position)
        {
            try
            {
                // This would require additional WinAPI calls for RemoveMenu
                // For now, return false to indicate not implemented
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing menu item: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets menu item information with proper error handling.
        /// </summary>
        public static bool SafeGetMenuItemInfo(IntPtr hMenu, uint item, bool byPosition, out MENUITEMINFO info)
        {
            info = new MENUITEMINFO();
            info.cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO));

            try
            {
                return GetMenuItemInfo(hMenu, item, byPosition, ref info);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting menu item info: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sets menu item information with proper error handling.
        /// </summary>
        public static bool SafeSetMenuItemInfo(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info)
        {
            try
            {
                return SetMenuItemInfo(hMenu, item, byPosition, ref info);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting menu item info: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region High DPI Menu Support

        /// <summary>
        /// Ensures menu items are properly scaled for High DPI displays.
        /// </summary>
        public static void ScaleMenuForDpi(ToolStrip menu, float dpiScale)
        {
            if (menu == null) return;

            menu.AutoSize = false;
            menu.Height = (int)(menu.Height * dpiScale);

            foreach (ToolStripItem item in menu.Items)
            {
                item.Height = (int)(item.Height * dpiScale);
                item.Width = (int)(item.Width * dpiScale);
                item.Font = new System.Drawing.Font(item.Font.FontFamily, item.Font.Size * dpiScale);
            }
        }

        /// <summary>
        /// Gets the current DPI scale factor.
        /// </summary>
        public static float GetDpiScale(Control control)
        {
            if (control == null) return 1.0f;

            using (var graphics = control.CreateGraphics())
            {
                return graphics.DpiX / 96.0f; // 96 is the standard DPI
            }
        }

        #endregion
    }
}