using System;
using System.Drawing;
using System.Windows.Forms;

namespace MacroCopyPaste
{
    public class CountdownForm : Form
    {
        private Label label;
        private Timer timer;
        private int secondsLeft;
        private int fadeStep = 20; // ms per fade step
        private int fadeSteps = 10; // number of fade steps between numbers
        private int fadeCounter = 0;
        private double fadeDelta;
        private bool fading = false;

        public CountdownForm(int seconds)
        {
            this.secondsLeft = seconds;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Opacity = 1.0;
            this.Width = 200;
            this.Height = 200;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            label = new Label()
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 64, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            this.Controls.Add(label);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ShowNumber();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!fading)
            {
                // Start fading out
                fading = true;
                fadeCounter = 0;
                fadeDelta = 1.0 / fadeSteps;
                timer.Interval = fadeStep;
            }
            else
            {
                fadeCounter++;
                this.Opacity -= fadeDelta;
                if (fadeCounter >= fadeSteps)
                {
                    // Done fading, show next number or close
                    secondsLeft--;
                    if (secondsLeft > 0)
                    {
                        this.Opacity = 1.0;
                        ShowNumber();
                        fading = false;
                        timer.Interval = 1000;
                    }
                    else
                    {
                        timer.Stop();
                        this.Close();
                    }
                }
            }
        }

        private void ShowNumber()
        {
            label.Text = secondsLeft.ToString();
        }
    }
}