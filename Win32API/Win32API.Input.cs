using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Win32API
{
	/// <summary>
	/// Modifier keys
	/// </summary>
	public enum ModKeys
	{
		/// <summary>
		/// No modifier
		/// </summary>
		None = 0,

		/// <summary>
		/// The SHIFT key is held down
		/// </summary>
		Shift = 0x01,

		/// <summary>
		/// The CONTROL key is held down
		/// </summary>
		Control = 0x02,

		/// <summary>
		/// The ALT key is held down
		/// </summary>
		Alt = 0x04
	};

	/// <summary>
	/// A helper class for keyboard and mouse input simulation
	/// </summary>
	public class Input
	{
		/// <summary>
		/// Move the mouse to a specified screen location
		/// </summary>
		/// <param name="x">X coords (relative to screen)</param>
		/// <param name="y">Y coords (relative to screen)</param>
		public static void MouseMove(int x, int y)
		{
			SetCursorPos(x, y);
		}

		/// <summary>
		/// Drag the mouse from one position to another
		/// </summary>
		/// <param name="x1">X coords of the start position</param>
		/// <param name="y1">Y coords of the start position</param>
		/// <param name="x2">X coords of the end position</param>
		/// <param name="y2">Y coords of the end position</param>
		/// <param name="button">The button to be held down</param>
		public static void MouseDrag(int x1, int y1, int x2, int y2, MouseButtons button = MouseButtons.Left)
		{
			MouseMove(x1, y1);
			MouseDown(button);
			MouseMove(x2, y2);
			MouseUp(button);
		}

		/// <summary>
		/// Press down a mouse button
		/// </summary>
		/// <param name="button">The button to be pressed</param>
		public static void MouseDown(MouseButtons button)
		{
			if ((button & MouseButtons.Left) != 0)
			{
				mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
			}

			if ((button & MouseButtons.Right) != 0)
			{
				mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
			}

			if ((button & MouseButtons.Middle) != 0)
			{
				mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
			}
		}


		/// <summary>
		/// Release a mouse button
		/// </summary>
		/// <param name="button">The button to be released</param>
		public static void MouseUp(MouseButtons button)
		{
			if ((button & MouseButtons.Left) != 0)
			{
				mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
			}

			if ((button & MouseButtons.Right) != 0)
			{
				mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
			}

			if ((button & MouseButtons.Middle) != 0)
			{
				mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
			}
		}

		/// <summary>
		/// Click a mouse button
		/// </summary>
		/// <param name="button">The button to be clicked</param>
		public static void MouseClick(MouseButtons button)
		{
			MouseDown(button);
			MouseUp(button);
		}

		/// <summary>
		/// Double-click a mouse button
		/// </summary>
		/// <param name="button">The button to be clicked</param>
		public static void MouseDblClick(MouseButtons button)
		{
			MouseClick(button);
			MouseClick(button);
		}

		/// <summary>
		/// Scroll the mouse wheel
		/// </summary>
		/// <param name="scrollUp">Scroll direction</param>
		public static void MouseWheel(bool scrollUp)
		{
			mouse_event(MOUSEEVENTF_WHEEL, 0, 0, scrollUp ? 120 : -120, 0);
		}

		/// <summary>
		/// Release all keys if held down
		/// </summary>
		public static void ReleaseAllKeys()
		{
			byte[] states = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				states[i] = 0;
			}

			GetKeyboardState(states);
			for (int i = 0; i < 256; i++)
			{
				if ((states[i] & 0x80) != 0)
				{
					keybd_event((byte)i, 0, 2, 0);
				}
			}
		}

		/// <summary>
		/// Remove redundant modifiers which already contained in key
		/// </summary>
		/// <param name="key">The key</param>
		/// <param name="mods">Modifiers</param>
		/// <returns></returns>
		public static ModKeys RemoveRedundantMods(Keys key, ModKeys mods)
		{
			if (mods == ModKeys.None)
			{
				return mods;
			}

			if (key == Keys.Shift)
				mods &= ~ModKeys.Shift;

			if (key == Keys.Control)
				mods &= ~ModKeys.Control;

			if (key == Keys.Alt)
				mods &= ~ModKeys.Alt;

			return mods;
		}

		/// <summary>
		/// Press down a key
		/// </summary>
		/// <param name="key">The key to be pressed down</param>
		/// <param name="mods">Modifiers</param>
		public static void KeyDown(Keys key, ModKeys mods = ModKeys.None)
		{
			mods = RemoveRedundantMods(key, mods);
			if ((mods & ModKeys.Alt) != 0)
			{
				keybd_event((byte)Keys.Menu, 0, 0, 0);
			}

			if ((mods & ModKeys.Control) != 0)
			{
				keybd_event((byte)Keys.ControlKey, 0, 0, 0);
			}

			if ((mods & ModKeys.Shift) != 0)
			{
				keybd_event((byte)Keys.ShiftKey, 0, 0, 0);
			}

			keybd_event((byte)key, 0, 0, 0);
		}

		/// <summary>
		/// Release a key
		/// </summary>
		/// <param name="key">The key to be released</param>
		/// <param name="mods">Modifiers</param>
		public static void KeyUp(Keys key, ModKeys mods = ModKeys.None)
		{
			mods = RemoveRedundantMods(key, mods);

			keybd_event((byte)key, 0, 2, 0);

			if ((mods & ModKeys.Shift) != 0)
			{
				keybd_event((byte)Keys.ShiftKey, 0, 2, 0);
			}

			if ((mods & ModKeys.Control) != 0)
			{
				keybd_event((byte)Keys.ControlKey, 0, 2, 0);
			}

			if ((mods & ModKeys.Alt) != 0)
			{
				keybd_event((byte)Keys.Menu, 0, 2, 0);
			}
		}

		/// <summary>
		/// Stroke a key
		/// </summary>
		/// <param name="key">The key to be stroked</param>
		/// <param name="mods">Modifiers</param>
		public static void KeyStroke(Keys key, ModKeys mods = ModKeys.None)
		{
			KeyDown(key, mods);
			KeyUp(key, mods);
		}

		[DllImport("user32.dll")]
		static extern int mouse_event(int flags, int x, int y, int button, int extraInfo);

		const int MOUSEEVENTF_MOVE = 0x0001;
		const int MOUSEEVENTF_LEFTDOWN = 0x0002;
		const int MOUSEEVENTF_LEFTUP = 0x0004;
		const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
		const int MOUSEEVENTF_RIGHTUP = 0x0010;
		const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
		const int MOUSEEVENTF_MIDDLEUP = 0x0040;
		const int MOUSEEVENTF_WHEEL = 0x800;
		const int MOUSEEVENTF_ABSOLUTE = 0x8000;

		/// <summary>
		/// Set cursor pos to specified screen location
		/// </summary>
		/// <param name="x">X coords (relative to screen)</param>
		/// <param name="y">Y coords (relative to screen)</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		static extern int GetCursorPos(out Point point);

		/// <summary>
		/// Retrieve the cursor location
		/// </summary>
		/// <returns>Return a Point struct contains X and Y coords relative to screen</returns>
		public static Point GetCursorPos()
		{
			Point point = new Point(0, 0);
			GetCursorPos(out point);
			return point;
		}

		[DllImport("user32.dll")]
		static extern int GetKeyboardState(byte[] states);

		[DllImport("user32.dll")]
		static extern void keybd_event(byte vkCode, byte scan, int flags, int extraInfo);

		[DllImport("user32.dll")]
		static extern short GetAsyncKeyState(Keys key);

		/// <summary>
		/// Chec k whether a specified key is currently held down
		/// </summary>
		/// <param name="key">The key</param>
		/// <returns>Return true if the key is held down, false otherwise</returns>
		public static bool IsKeyDown(Keys key)
		{
			return (GetAsyncKeyState(key) & 0x8000) != 0;
		}
	}
}
