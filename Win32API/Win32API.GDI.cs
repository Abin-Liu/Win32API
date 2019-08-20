using System;
using System.Runtime.InteropServices;

namespace Win32API
{
	/// <summary>
	/// A helper class for screen pixel capture
	/// </summary>
	public class GDI
	{
		/// <summary>
		/// Retrieve a handle of the device context to a window
		/// </summary>
		/// <param name="hwnd">Handle of the window, the desktop window if the parameter is IntPtr.Zero</param>
		/// <returns>Return handle of the device context if successful, or IntPtr.Zero if failed</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);

		/// <summary>
		/// Release a device context
		/// </summary>
		/// <param name="hwnd">Handle of the window, the desktop window if the parameter is IntPtr.Zero</param>
		/// <param name="hdc">Handle of the device context</param>
		/// <returns>Return true if successful, false otherwise</returns>
		[DllImport("user32.dll")]
		public static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("Gdi32.dll", EntryPoint = "GetPixel")]
		private static extern int Win32GetPixel(IntPtr hdc, int x, int y);

		/// <summary>
		/// Retrieve pixel of the screen
		/// </summary>
		/// <param name="hdc">Handle of the device context</param>
		/// <param name="x">X coords relative to screen</param>
		/// <param name="y">Y coords relative to screen</param>
		/// <returns>Return RGB value if successful, -1 if failed</returns>
		public static int GetPixel(IntPtr hdc, int x, int y)
		{
			int color = Win32GetPixel(hdc, x, y);			

			// RGB components from win32 GetPixel are reversed from C# .net System.Drawing.Color
			byte r = GetRValue(color);
			byte g = GetGValue(color);
			byte b = GetBValue(color);
			return RGB(b, g, r);
		}

		/// <summary>
		/// Compose a RGB value
		/// </summary>
		/// <param name="r">The red component</param>
		/// <param name="g">The green component</param>
		/// <param name="b">The blue component</param>
		/// <returns>Return the composed RGB value</returns>
		public static int RGB(byte r, byte g, byte b)
		{
			return ((int)r << 16) | ((short)g << 8) | b; // It's C# .net System.Drawing.Color way
		}

		/// <summary>
		/// Extract the red component of a RGB value
		/// </summary>
		/// <param name="color">The RGB value</param>
		/// <returns>Return the red component</returns>
		public static byte GetRValue(int color)
		{
			return (byte)(color >> 16);
		}

		/// <summary>
		/// Extract the green component of a RGB value
		/// </summary>
		/// <param name="color">The RGB value</param>
		/// <returns>Return the green component</returns>
		public static byte GetGValue(int color)
		{
			return (byte)(((short)color) >> 8);
		}

		/// <summary>
		/// Extract the blue component of a RGB value
		/// </summary>
		/// <param name="color">The RGB value</param>
		/// <returns>Return the blue component</returns>
		public static byte GetBValue(int color)
		{
			return (byte)color;
		}
	}
}
