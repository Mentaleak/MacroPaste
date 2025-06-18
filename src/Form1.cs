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

namespace MacroCopyPaste
{
    /// <summary>
    /// Represents the main form for configuring hotkeys in the MacroCopyPaste application.
    /// </summary>
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

        private TrayAppContext trayAppContext;

        /// <summary>
        /// Initializes a new instance of the Form1 class.
        /// </summary>
        public Form1(TrayAppContext trayAppContext)
        {
            InitializeComponent();
            this.trayAppContext = trayAppContext;
        }

        private Keys currentHotkey;
        private Keys currentModifiers;
        private Keys defaultKey = Keys.V;
        private Keys defaultModifiers = Keys.Control | Keys.Alt;

        /// <summary>
        /// Handles the KeyDown event for the hotkey text box, allowing users to configure a custom hotkey.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The KeyEventArgs containing event data.</param>
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

        /// <summary>
        /// Handles the Load event for the form, initializing the default hotkey and registering it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs containing event data.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            currentHotkey = defaultKey;
            currentModifiers = defaultModifiers;

            textBox_HotKey.Text = "Ctrl + Alt + V";

            RegisterSelectedHotkey(); // Optional: auto-register on launch
        }

        /// <summary>
        /// Registers the currently selected hotkey with the operating system.
        /// </summary>
        private void RegisterSelectedHotkey()
        {
            uint modifiers = 0;
            if ((currentModifiers & Keys.Control) == Keys.Control) modifiers |= 0x0002;
            if ((currentModifiers & Keys.Alt) == Keys.Alt) modifiers |= 0x0001;
            if ((currentModifiers & Keys.Shift) == Keys.Shift) modifiers |= 0x0004;

            const int HOTKEY_ID = 9000;
            RegisterHotKey(this.Handle, HOTKEY_ID, modifiers, (uint)currentHotkey);
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            // Update the delay value in TrayAppContext
            int newDelay = (int)numericUpDown_Delay.Value;
            trayAppContext.UpdateDelay(newDelay);

            MessageBox.Show("Delay updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosing(e);
        }
    }
}
