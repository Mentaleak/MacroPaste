using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MacroCopyPaste
{
    /// <summary>
    /// Represents the application context for the tray application, managing the tray icon and hotkey functionality.
    /// </summary>
    public class TrayAppContext : ApplicationContext
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;
        private const int WM_KEYUP = 0x0101; // Windows message for key release

        private NotifyIcon trayIcon;
        private int delayInSeconds;

        /// <summary>
        /// Initializes the tray application context, including the tray icon, context menu, and hotkey registration.
        /// </summary>
        public TrayAppContext(int delayInSeconds)
        {
            this.delayInSeconds = delayInSeconds;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Set-Hot-Key", null, SetHotKey);
            contextMenu.Items.Add("Exit", null, Exit);

            // Use the form's icon for the tray icon
            Icon formIcon = new Form1(this).Icon;

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

        /// <summary>
        /// Updates the delay in seconds for clipboard typing simulation.
        /// </summary>
        /// <param name="newDelayInSeconds">The new delay in seconds.</param>
        public void UpdateDelay(int newDelayInSeconds)
        {
            delayInSeconds = newDelayInSeconds;
        }

        /// <summary>
        /// Unregisters the hotkey and performs cleanup when the application exits.
        /// </summary>
        protected override void ExitThreadCore()
        {
            UnregisterHotKey(IntPtr.Zero, HOTKEY_ID);
            base.ExitThreadCore();
        }

        /// <summary>
        /// Opens the hotkey configuration form when the "Set-Hot-Key" menu item is clicked.
        /// </summary>
        private void SetHotKey(object sender, EventArgs e)
        {
            Form1 form = new Form1(this); // Pass the current TrayAppContext instance
            form.ShowDialog();
        }

        /// <summary>
        /// Exits the application when the "Exit" menu item is clicked.
        /// </summary>
        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        /// <summary>
        /// Checks if a specific key is currently pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is pressed; otherwise, false.</returns>
        private bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }

        /// <summary>
        /// Checks if any modifier key (Ctrl, Shift, Alt) is currently pressed.
        /// </summary>
        /// <returns>True if a modifier key is pressed; otherwise, false.</returns>
        private bool IsModifierKeyDown()
        {
            return IsKeyDown(Keys.ControlKey) || IsKeyDown(Keys.ShiftKey) || IsKeyDown(Keys.Menu); // Alt
        }

        /// <summary>
        /// Escapes special characters for use with SendKeys.
        /// </summary>
        /// <param name="c">The character to escape.</param>
        /// <returns>The escaped character string.</returns>
        private string EscapeSendKeysChar(char c)
        {
            switch (c)
            {
                case '+': return "{+}";
                case '^': return "{^}";
                case '%': return "{%}";
                case '~': return "{~}";
                case '(': return "{(}";
                case ')': return "{)}";
                case '[': return "{[}";
                case ']': return "{]}";
                case '{': return "{{}";
                case '}': return "{}}";
                default: return c.ToString();
            }
        }

        /// <summary>
        /// Simulates typing the text currently stored in the clipboard.
        /// </summary>
        private void SimulateClipboardTyping()
        {
            System.Threading.Thread.Sleep(delayInSeconds * 1000);

            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

                foreach (char c in clipboardText)
                {
                    int attempts = 0;
                    while (IsModifierKeyDown())
                    {
                        System.Threading.Thread.Sleep(50);
                        attempts++;
                        if (attempts >= 10 && IsModifierKeyDown())
                        {
                            MessageBox.Show("Modifier key is still pressed after 10 attempts. Exiting loop.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string keyToSend = EscapeSendKeysChar(c);
                    SendKeys.SendWait(keyToSend);
                }
            }
            else
            {
                MessageBox.Show("Clipboard does not contain text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Filters Windows messages to detect hotkey events and invoke the associated action.
    /// </summary>
    public class HotKeyMessageFilter : IMessageFilter
    {
        private readonly int hotKeyId;
        private readonly int keyReleaseMessage;
        private readonly Action hotKeyAction;

        /// <summary>
        /// Initializes a new instance of the HotKeyMessageFilter class.
        /// </summary>
        /// <param name="hotKeyId">The ID of the hotkey to filter.</param>
        /// <param name="keyReleaseMessage">The Windows message to filter for key release.</param>
        /// <param name="hotKeyAction">The action to invoke when the hotkey is detected.</param>
        public HotKeyMessageFilter(int hotKeyId, int keyReleaseMessage, Action hotKeyAction)
        {
            this.hotKeyId = hotKeyId;
            this.keyReleaseMessage = keyReleaseMessage;
            this.hotKeyAction = hotKeyAction;
        }

        /// <summary>
        /// Filters Windows messages to detect the specified hotkey event.
        /// </summary>
        /// <param name="m">The Windows message to filter.</param>
        /// <returns>True if the message was handled; otherwise, false.</returns>
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
