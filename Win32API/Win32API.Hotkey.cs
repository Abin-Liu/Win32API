using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Win32API
{
	/// <summary>
	/// A helper class for system hot key registration
	/// </summary>
	public class Hotkey
	{
		const int WM_HOTKEY = 0x0312;
		const int MOD_ALT = 0x01;
		const int MOD_CONTROL = 0x02;
		const int MOD_SHIFT = 0x04;

		[DllImport("user32.dll")]
		static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, Keys vk);

		/// <summary>
		/// Register a hotkey
		/// </summary>
		/// <param name="hWnd">Handle of the window which will receive WM_HOTKEY event</param>
		/// <param name="id">Unique identity of the hotkey</param>
		/// <param name="vk">Key</param>
		/// <param name="mods">Modifiers, default is none</param>
		/// <returns></returns>
		public static bool RegisterHotKey(IntPtr hWnd, int id, Keys vk, ModKeys mods = ModKeys.None)
		{
			int flags = 0;
			if ((mods & ModKeys.Alt) != 0)
			{
				flags |= MOD_ALT;
			}

			if ((mods & ModKeys.Shift) != 0)
			{
				flags |= MOD_SHIFT;
			}

			if ((mods & ModKeys.Control) != 0)
			{
				flags |= MOD_CONTROL;
			}

			return RegisterHotKey(hWnd, id, flags, vk);
		}

		/// <summary>
		/// Unregister a hotkey
		/// </summary>
		/// <param name="hWnd">Handle of the window which was used to register the hotkey</param>
		/// <param name="id">Unique identity of the hotkey</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		/// <summary>
		/// Check whether a window message is a WM_HOTKEY
		/// </summary>
		/// <param name="m">The message</param>
		/// <returns>Return the unique identity of the hotkey if it's a WM_HOTKEY, return -1 otherwise</returns>
		public static int IsHotkeyEvent(ref Message m)
		{
			if (m.Msg == WM_HOTKEY)
			{
				return (int)m.WParam;
			}

			return -1;						
		}
	}
}
