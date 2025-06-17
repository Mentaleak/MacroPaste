using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MacroCopyPaste
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;

        // Modifier constants
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        private List<string> commonlyUsedHotkeys = new List<string>
        {
            "Ctrl + C",
            "Ctrl + V",
            "Ctrl + Alt + Delete"
        };

        public Form1()
        {
            InitializeComponent();
        }
        private Keys currentHotkey;
        private Keys currentModifiers;
        private Keys defaultKey = Keys.V;
        private Keys defaultModifiers = Keys.Control | Keys.Alt;
        private void textBox_HotKey_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            // Get modifier keys
            currentModifiers = Keys.None;
            if (e.Control) currentModifiers |= Keys.Control;
            if (e.Alt) currentModifiers |= Keys.Alt;
            if (e.Shift) currentModifiers |= Keys.Shift;

            // Get the main key
            currentHotkey = e.KeyCode;

            // Display in textbox
            string hotkeyText = $"{(e.Control ? "Ctrl + " : "")}" +
                                $"{(e.Alt ? "Alt + " : "")}" +
                                $"{(e.Shift ? "Shift + " : "")}" +
                                $"{e.KeyCode}";
            textBox_HotKey.Text = hotkeyText;

            // Check if the hotkey is commonly used
            if (commonlyUsedHotkeys.Contains(hotkeyText))
            {
                textBox_HotKey.BackColor = Color.Red; // Highlight in red
                button_update.Enabled = false;       // Disable the update button
            }
            else
            {
                textBox_HotKey.BackColor = Color.White; // Reset to default color
                button_update.Enabled = true;          // Enable the update button
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            currentHotkey = defaultKey;
            currentModifiers = defaultModifiers;

            textBox_HotKey.Text = "Ctrl + Alt + V";

            RegisterSelectedHotkey(); // Optional: auto-register on launch
        }

        private void RegisterSelectedHotkey()
        {
            uint modifiers = 0;
            if ((currentModifiers & Keys.Control) == Keys.Control) modifiers |= 0x0002;
            if ((currentModifiers & Keys.Alt) == Keys.Alt) modifiers |= 0x0001;
            if ((currentModifiers & Keys.Shift) == Keys.Shift) modifiers |= 0x0004;

            const int HOTKEY_ID = 9000;
            RegisterHotKey(this.Handle, HOTKEY_ID, modifiers, (uint)currentHotkey);
        }
        private void RegisterPasteHotkey()
        {
            uint modifiers = MOD_CONTROL | MOD_ALT;
            uint vk = (uint)Keys.V;

            bool registered = RegisterHotKey(this.Handle, HOTKEY_ID, modifiers, vk);

            if (!registered)
            {
                MessageBox.Show("Failed to register hotkey. It might already be in use.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_update_Click(object sender, EventArgs e)
        {
            // Unregister the current hotkey
            UnregisterHotKey(this.Handle, HOTKEY_ID);

            // Register the new hotkey
            RegisterSelectedHotkey();

            MessageBox.Show("Hotkey updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosing(e);
        }

        private void SimulateClipboardTyping()
        {
            // Check if the clipboard contains text
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

                // Simulate typing the clipboard text
                foreach (char c in clipboardText)
                {
                    SendKeys.Send(c.ToString());
                }
            }
            else
            {
                MessageBox.Show("Clipboard does not contain text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                if (id == HOTKEY_ID)
                {
                    // Check if textBox_HotKey has focus
                    if (!textBox_HotKey.Focused)
                    {
                        // Perform the hotkey action only if textBox_HotKey does not have focus
                        SimulateClipboardTyping();
                    }
                }
            }
            base.WndProc(ref m);
        }
    }
}
