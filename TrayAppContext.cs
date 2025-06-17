using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MacroCopyPaste
{
    public class TrayAppContext : ApplicationContext
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;
        private const int WM_KEYUP = 0x0101; // Windows message for key release

        private NotifyIcon trayIcon;

        public TrayAppContext()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Set-Hot-Key", null, SetHotKey);
            contextMenu.Items.Add("Exit", null, Exit);

            // Use the form's icon for the tray icon
            Icon formIcon = new Form1().Icon;

            trayIcon = new NotifyIcon()
            {
                Icon = formIcon, // Set the tray icon to the form's icon
                ContextMenuStrip = contextMenu,
                Visible = true,
                Text = "Hotkey Tray App"
            };

            // Register the default hotkey (Ctrl + Alt + V)
            RegisterHotKey(IntPtr.Zero, HOTKEY_ID, 0x0003, (uint)Keys.V); // MOD_CONTROL | MOD_ALT

            // Add a message filter to handle WM_HOTKEY messages
            Application.AddMessageFilter(new HotKeyMessageFilter(HOTKEY_ID, WM_HOTKEY, SimulateClipboardTyping));

            // Add a message filter to handle WM_KEYUP messages for key release
            Application.AddMessageFilter(new HotKeyMessageFilter(HOTKEY_ID, WM_KEYUP, SimulateClipboardTyping));
        }

        protected override void ExitThreadCore()
        {
            // Unregister the hotkey when the application exits
            UnregisterHotKey(IntPtr.Zero, HOTKEY_ID);
            base.ExitThreadCore();
        }

        private void SetHotKey(object sender, EventArgs e)
        {
            // Launch the hotkey form
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void SimulateClipboardTyping()
        {
            // Check if the clipboard contains text
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

                // Simulate typing the clipboard text while preserving case
                foreach (char c in clipboardText)
                {
					SendKeys.SendWait(c.ToString());
                }

            }
            else
            {
                MessageBox.Show("Clipboard does not contain text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // New class to handle WM_HOTKEY messages
    public class HotKeyMessageFilter : IMessageFilter
    {
        private readonly int hotKeyId;
        private readonly int keyReleaseMessage;
        private readonly Action hotKeyAction;

        public HotKeyMessageFilter(int hotKeyId, int keyReleaseMessage, Action hotKeyAction)
        {
            this.hotKeyId = hotKeyId;
            this.keyReleaseMessage = keyReleaseMessage;
            this.hotKeyAction = hotKeyAction;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == keyReleaseMessage && m.WParam.ToInt32() == hotKeyId)
            {
                hotKeyAction.Invoke();
                return true;
            }
            return false;
        }
    }
}
