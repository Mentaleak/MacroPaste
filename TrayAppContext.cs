using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacroCopyPaste
{
	public class TrayAppContext : ApplicationContext
	{
		private NotifyIcon trayIcon;

		public TrayAppContext()
		{
			var contextMenu = new ContextMenuStrip();
			contextMenu.Items.Add("Set-Hot-Key", null, SetHotKey);
			contextMenu.Items.Add("Exit", null, Exit);

			trayIcon = new NotifyIcon()
			{
				Icon = SystemIcons.Application, // You can replace with your own icon
				ContextMenuStrip = contextMenu,
				Visible = true,
				Text = "Hotkey Tray App"
			};
		}

		private void SetHotKey(object sender, EventArgs e)
		{
			 // Launch the hotkey form without starting a new message loop
			Form1 form = new Form1();
			form.ShowDialog(); // or form.Show();
		}

		private void Exit(object sender, EventArgs e)
		{
			trayIcon.Visible = false;
			Application.Exit();
		}
	}
}
