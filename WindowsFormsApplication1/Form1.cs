using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;

namespace BulletHellFish
{
    public partial class BulletHellFishForm : Form
    {
        //Imports
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        private string EmuWindowTitle;

        long startGameMS;
        long startRealMS;
        long startWorldMS;

        Player player1 = new Player(1);
        Player player2 = new Player(2);

        PlayerFormControls player1Controls;
        PlayerFormControls player2Controls;

        IntPtr GameEmuHandle = IntPtr.Zero;
        
        public BulletHellFishForm()
        {
            InitializeComponent();
            
            // Standard Window
            WindowNameDropDown.Text = "pcsx2-r4600";
            
            player1Controls = new PlayerFormControls(this, Player1FormControls, openFileDialog1);
            player2Controls = new PlayerFormControls(this, Player2FormControls, openFileDialog1);

            // Game Window List
            RefreshWindowNamesCombo();

            player1Controls.InitPlayer(player1);
            player2Controls.InitPlayer(player2);

            LoadChart();
        }
        

        internal InputParameters GetInputParameters(Hand hand)
        {
            bool leftSide = hand == Hand.Left;
            return new InputParameters( Utils.IntFromTextBox(leftSide ? HoldTimeLeftFrom : HoldTimeRightFrom, 50),
                                        Utils.IntFromTextBox(leftSide ? HoldTimeLeftTo : HoldTimeRightTo, 50),
                                        Utils.IntFromTextBox(leftSide ? SleepTimeLeftTextBox : SleepTimeRightTextBox, 0));
        }

        private void StartButtonClicked(object sender, EventArgs e)
        {
            if (!player1.IsPlaying()) { Start(); }
            else { Pause(); }
        }

        private IntPtr GetEmuWindowHandle(string windowTitle)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(windowTitle) && windowTitle.Equals(process.MainWindowTitle))
                {
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        private void Start()
        {
            bool TwoPlayersMode = true;

            // Emu Window
            EmuWindowTitle = WindowNameDropDown.Text;
            GameEmuHandle = FindWindow(null, EmuWindowTitle);

            if (GameEmuHandle == IntPtr.Zero)
            {
                // Try again
                Process[] processes = Process.GetProcessesByName(EmuWindowTitle);
                if (processes != null && processes.Length > 0 && processes[0].MainWindowHandle != IntPtr.Zero)
                {
                    GameEmuHandle = processes[0].MainWindowHandle;
                } else { 
                    MessageBox.Show("Window not found");
                    return;
                }
            }

            // InputBoard Behaviors
            player1Controls.LoadBehaviors(GameEmuHandle, SaveShotPathTextBox.Text);
            player2Controls.LoadBehaviors(GameEmuHandle, SaveShotPathTextBox.Text);


            // Views
            StartPauseButton.Text = "Pause";
            SpeedMultiplierTextBox.Enabled = false;
            SetForegroundWindow(GameEmuHandle);

            startGameMS = Utils.parseHourToMilliseconds(FastClockTextBox);
            startRealMS = Utils.parseHourToMilliseconds(NormalClockTextBox);
            startWorldMS = Utils.millisecondsSinceEpoch();

            // Input threads
            player1.Play(GameEmuHandle, this, true, player1Controls.GetSource());
            
            if(TwoPlayersMode)
            {
                player2.Play(GameEmuHandle, this, false, player2Controls.GetSource());
            }
            
        }

        public void UpdateForm(string inputName, Hand hand, bool main, int playerId, int dataCursor)
        {
            PlayerFormControls controls = playerId == 1 ? player1Controls : player2Controls;
            Label label = controls.GetInputLabel(hand);
            ListBox history = controls.HistoryListBox;

            // Common Data
            int multiplier = Utils.IntFromTextBox(SpeedMultiplierTextBox, 1);

            Action action = new Action(() =>
            {
                label.Text = inputName;

                if (!string.IsNullOrEmpty(inputName))
                {
                    history.Items.Add(hand + " " + inputName);
                    int visibleItems = history.ClientSize.Height / history.ItemHeight;
                    history.TopIndex = Math.Max(history.Items.Count - visibleItems + 1, 0);
                }

                SetCursor(dataCursor, playerId, hand);

                if (main)
                {
                    long secondsElapsed = Utils.millisecondsSinceEpoch() - startWorldMS;
                    Utils.fillWithDate(NormalClockTextBox, secondsElapsed + startRealMS);
                    Utils.fillWithDate(FastClockTextBox, secondsElapsed * multiplier + startGameMS);
                }
            });

            Invoke(action);
        }
        

        public void Pause()
        {
            Invoke(new Action(() =>
            {
                SpeedMultiplierTextBox.Enabled = true;

                StartPauseButton.Text = "Start";

                if (player1 != null) player1.Pause();
                if (player2 != null) player2.Pause();

            }));
        }
      
        private void SaveStartScreenButton_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(GameEmuHandle);
            ScreenshotManager.SaveScreenAs(SaveShotPathTextBox.Text, ScreenshotManager.GenerateScreenPath(SaveShotPathTextBox.Text, SaveShotNameTextBox.Text), GameEmuHandle);
        }

        private void ClearStartScreenButton_Click(object sender, EventArgs e)
        {
            ScreenshotManager.ClearScreens(SaveShotPathTextBox.Text, ClearShotNameTextBox.Text);
        }

        private void RefreshWindowNamesCombo()
        {
            WindowNameDropDown.Items.Clear();
            Process[] processlist = Process.GetProcesses();


            WindowNameDropDown.Items.Add(" - Window Names - ");

            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    WindowNameDropDown.Items.Add(process.MainWindowTitle);
                }
            }

            WindowNameDropDown.Items.Add("  ");
            WindowNameDropDown.Items.Add(" - Processes - ");

            List<string> strings = new List<string>();

            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.ProcessName))
                {
                    strings.Add(process.ProcessName);
                }
            }

            strings.Sort();

            foreach (string process in strings)
            {
                WindowNameDropDown.Items.Add(process);
            }
        }
        

        #region Window Styling

        private void backColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                setBackColor(colorDialog1.Color);
            }
        }

        private void textColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                setForeColor(colorDialog1.Color);
            }
        }

        private void setBackColor(Color color)
        {
            IEnumerable<Control> views = Utils.GetAllControls(this, true);

            setBackColor(color, views);
        }

        private void setForeColor(Color color)
        {

            IEnumerable<Control> views = Utils.GetAllControls(this, true);
            setForeColor(color, views);

        }

        private void setForeColor(Color color, IEnumerable<Control> views) {

            foreach (Control control in views) {
                control.ForeColor = color;
            }

          
        }

        private void setBackColor(Color color, IEnumerable<Control> views)
        {
            foreach (Control control in views)
            {
                control.BackColor = colorDialog1.Color;
            }
        }
        
        #endregion
      
        private void WindowNameDropDown_OnClick(object sender, EventArgs e)
        {
            RefreshWindowNamesCombo();
        }


        #region Input Source


        public void LoadChart()
        {
            chart1.Series["Player1Source"].Points.Clear();
            chart1.Series["Player2Source"].Points.Clear();

            List<double> s1 = player1Controls.GetSource();
            List<double> s2 = player2Controls.GetSource();
            double m = 0;

            for (int i = 0; i < Math.Max(s1.Count, s2.Count); i++)
            {
                if (i < s1.Count)
                {
                    chart1.Series["Player1Source"].Points.AddXY(i, s1[i]);
                    m = Math.Max(s1[i], m);
                }
                if (i < s2.Count)
                {
                    chart1.Series["Player2Source"].Points.AddXY(i, s2[i]);
                    m = Math.Max(s2[i], m);
                }
            }
            
        }

        public void SetCursor(int position, int playerId, Hand hand)
        {
            if (hand == Hand.Left) return;

            string name = "Cursor" + playerId + hand;

            chart1.Series[name].Points.Clear();
            chart1.Series[name].Points.AddXY(position, 0);
            chart1.Series[name].Points.AddXY(position, 1);
        }


        #endregion

        private void LiftButton_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(GameEmuHandle);
            player1.InputBoard.LiftAll();
            player2.InputBoard.LiftAll();
        }
    }
}
