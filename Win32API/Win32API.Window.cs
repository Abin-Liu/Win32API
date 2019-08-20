using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32API
{
	/// <summary>
	/// a helper class for window manipulation
	/// </summary>
	public class Window
	{
		/// <summary>
		/// Find a specified window
		/// </summary>
		/// <param name="className">Class name of the window, does not exam if null</param>
		/// <param name="windowName">Title text of the window, does not exam if null</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public extern static IntPtr FindWindow(string className, string windowName);

		/// <summary>
		/// Check whether a window handle is valid
		/// </summary>
		/// <param name="hwnd">The handle to be examed</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public extern static bool IsWindow(IntPtr hwnd);

		/// <summary>
		/// Delegate for EnumWindows, it will be called for every window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <param name="lParam">User defined parameter</param>
		/// <returns></returns>
		public delegate bool EnumWindowsCallBack(IntPtr hwnd, int lParam);

		/// <summary>
		/// Enumerate all top-level windows
		/// </summary>
		/// <param name="callback">The delegate to be called when every window is found</param>
		/// <param name="lParam">User defined parameter</param>
		/// <returns>Return true if the enumeration completed successfully, false otherwise</returns>
		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsCallBack callback, int lParam);		
		

		[DllImport("user32.dll")]
		static extern bool GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

		/// <summary>
		/// Retrieve title text of a window
		/// </summary>
		/// <param name="hWnd">Handle of the window</param>
		/// <returns>Return title text if the handle is valid, null otherwise</returns>
		public static string GetWindowText(IntPtr hWnd)
		{
			StringBuilder sb = new StringBuilder(1000);
			if (!GetWindowText(hWnd, sb, sb.Capacity))
			{
				return null;
			}

			return sb.ToString();
		}

		[DllImport("user32.dll")]
		static extern bool GetClassName(IntPtr hWnd, StringBuilder text, int nMaxCount);

		/// <summary>
		/// Retrieve class name of a window
		/// </summary>
		/// <param name="hWnd">Handle of the window</param>
		/// <returns>Return class name if the handle is valid, null otherwise</returns>
		public static string GetClassName(IntPtr hWnd)
		{
			StringBuilder sb = new StringBuilder(1000);
			if (!GetClassName(hWnd, sb, sb.Capacity))
			{
				return null;
			}

			return sb.ToString();
		}

		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

		/// <summary>
		/// Retrieve boundary rectangle of a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return boundary rectangle if the handle is valid, return an empty rectangle otherwise</returns>
		public static Rectangle GetWindowRect(IntPtr hwnd)
		{
			Rectangle rect = new Rectangle();
			GetWindowRect(hwnd, out rect);
			return rect;
		}

		[DllImport("user32.dll")]
		static extern bool GetClientRect(IntPtr hwnd, out Rectangle lpRect);

		/// <summary>
		/// Retrieve boundary rectangle of the client area a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return boundary rectangle of the client area if the handle is valid, return an empty rectangle otherwise</returns>
		public static Rectangle GetClientRect(IntPtr hwnd)
		{
			Rectangle rect = new Rectangle();
			GetClientRect(hwnd, out rect);
			return rect;
		}

		[DllImport("user32.dll", EntryPoint = "ClientToScreen")]
		static extern bool _ClientToScreen(IntPtr hwnd, out Point lpPoint);

		/// <summary>
		/// Retrieve client-to-screen offset of a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return a Point struct which contain X and Y offsets if the handle is valid, return an empty Point otherwise</returns>
		public static Point ClientToScreen(IntPtr hwnd)
		{
			Point offset;
			_ClientToScreen(hwnd, out offset);			
			return offset;
		}

		[DllImport("user32.dll", EntryPoint = "ScreenToClient")]
		static extern bool _ScreenToClient(IntPtr hwnd, out Point lpPoint);

		/// <summary>
		/// Retrieve screen-to-client offset of a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return a Point struct which contain X and Y offsets if the handle is valid, return an empty Point otherwise</returns>

		public static Point ScreenToClient(IntPtr hwnd)
		{
			Point offset;
			_ScreenToClient(hwnd, out offset);
			return offset;
		}

		/// <summary>
		/// Retrieve window-to-screen offset of a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return a Point struct which contain X and Y offsets if the handle is valid, return an empty Point otherwise</returns>

		public static Point WindowToScreen(IntPtr hwnd)
		{
			Point offset = new Point(0, 0);
			Rectangle rect = new Rectangle();
			if (GetWindowRect(hwnd, out rect))
			{
				offset.X = rect.X;
				offset.Y = rect.Y;
			}

			return offset;
		}

		/// <summary>
		/// Retrieve screen-to-window offset of a window
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return a Point struct which contain X and Y offsets if the handle is valid, return an empty Point otherwise</returns>

		public static Point ScreenToWindow(IntPtr hwnd)
		{
			Point offset = WindowToScreen(hwnd);
			offset.X = -offset.X;
			offset.Y = -offset.Y;
			return offset;
		}

		/// <summary>
		/// Retrieve the foreground window
		/// </summary>
		/// <returns>Handle of the foreground window</returns>
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		/// <summary>
		/// Set a window to foreground
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hwnd);

		/// <summary>
		/// Check whether a window is miminized
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return true if the specified window is minimized, false otherwise</returns>
		[DllImport("user32.dll", EntryPoint = "IsIconic")]
		public static extern bool IsMinimized(IntPtr hwnd);

		/// <summary>
		/// Check whether a window is maximized
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return true if the specified window is maxmized, false otherwise</returns>
		[DllImport("user32.dll", EntryPoint = "IsZoomed")]
		public static extern bool IsMaximized(IntPtr hwnd);

		/// <summary>
		/// Check whether a window is visible
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <returns>Return true if the window is visible, false otherwise</returns>
		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		/// <summary>
		/// Display the window in specified way
		/// </summary>
		/// <param name="hwnd">Handle of the window</param>
		/// <param name="nCmdShow">How the window will be displayed</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

		/// <summary>
		/// The window will be hidden
		/// </summary>
		public const int SW_HIDE = 0;

		/// <summary>
		/// The window will be shown normally and activated
		/// </summary>
		public const int SW_SHOWNORMAL = 1;

		/// <summary>
		/// The window will be shown normally and activated
		/// </summary>
		public const int SW_NORMAL = 1;

		/// <summary>
		/// The window will be shown minimized and activated
		/// </summary>
		public const int SW_SHOWMINIMIZED = 2;

		/// <summary>
		/// The window will be shown maximized and activated
		/// </summary>
		public const int SW_SHOWMAXIMIZED = 3;

		/// <summary>
		/// The window will be shown maximized and activated
		/// </summary>
		public const int SW_MAXIMIZE = 3;

		/// <summary>
		/// The window will be shown in recent state, but will not be activated
		/// </summary>
		public const int SW_SHOWNOACTIVATE = 4;

		/// <summary>
		/// The window will be shown normally and activated
		/// </summary>
		public const int SW_SHOW = 5;

		/// <summary>
		/// The window will be shown minimized, but will not be activated
		/// </summary>
		public const int SW_MINIMIZE = 6;

		/// <summary>
		/// The window will be shown minimized, but will not be activated
		/// </summary>
		public const int SW_SHOWMINNOACTIVE = 7;

		/// <summary>
		/// The window will be shown in recent state, but will not be activated
		/// </summary>
		public const int SW_SHOWNA = 8;

		/// <summary>
		/// The window will be shown normally and activated
		/// </summary>
		public const int SW_RESTORE = 9;

		/// <summary>
		/// The window will be shown normally and activated
		/// </summary>
		public const int SW_SHOWDEFAULT = 10;

		/// <summary>
		/// Retrieve handle of the desktop window
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public extern static IntPtr GetDesktopWindow();

		/// <summary>
		/// The win32 WM_USER, private window messages should be defined between WM_USER and 0x7FFF
		/// </summary>
		public const int WM_USER = 0x0400;

		/// <summary>
		/// The win32 WM_APP, global window messages should be defined between WM_APP and 0xBFFF
		/// </summary>
		public const int WM_APP = 0x8000;

		/// <summary>
		/// Send a message to a window, the function will not return until the event handler returns
		/// </summary>
		/// <param name="hWnd">Handle of the window which will receive the message</param>
		/// <param name="Msg">Message identity</param>
		/// <param name="wParam">Win32 WPARAM</param>
		/// <param name="lParam">Win32 LPARAM</param>
		/// <returns>Return the value returned by the event handler</returns>
		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		/// <summary>
		/// Post a message to a window, the function returns immediately
		/// </summary>
		/// <param name="hWnd">Handle of the window which will receive the message</param>
		/// <param name="Msg">Message identity</param>
		/// <param name="wParam">Win32 WPARAM</param>
		/// <param name="lParam">Win32 LPARAM</param>
		/// <returns>Return non-zero if the function successful, return zero if failed</returns>
		[DllImport("User32.dll")]
		public static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);		
	}
}
