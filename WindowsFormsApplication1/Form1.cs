using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
        private int OriginalLeftInputLabelWidth;

        Player player1;// = new Player(new PSXInputBoard("C:\\Users\\simc\\Desktop\\The Bullet Hell Fish\\Input Sheets\\PSX Input Sheet P1.csv"));
        Player player2;// = new Player(new PSXInputBoard("C:\\Users\\simc\\Desktop\\The Bullet Hell Fish\\Input Sheets\\PSX Input Sheet P2.csv"));

        IntPtr GameEmuHandle = IntPtr.Zero;


        public BulletHellFishForm()
        {
            InitializeComponent();
            
            // StdWindow
            WindowNameDropDown.Text = "PCSXR";

            InitPlayers();
        }

        private void InitPlayers()
        {
            if (!string.IsNullOrWhiteSpace(Controller1MappingTextBox.Text))
                player1 = new Player(new PSXInputBoard(Controller1MappingTextBox.Text));

            if (!string.IsNullOrWhiteSpace(Controller2MappingTextBox.Text))
                player2 = new Player(new PSXInputBoard(Controller2MappingTextBox.Text));

            if (player1 == null)
                return;

            // Create CheckedListbox
            AllowedInputListBox.Items.Clear();
            foreach (string input in player1.InputBoard.EveryInput())
            {
                AllowedInputListBox.Items.Add(input);
                AllowedInputListBox.SetItemChecked(AllowedInputListBox.Items.IndexOf(input), true);
            }

            // Window List
            RefreshComboBox();

            // TwoHandMode
            OriginalLeftInputLabelWidth = LeftInputLabel.Width;
            TwoHandsCheckBox_CheckedChanged(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
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

            // InputBoard Behaviors: Only for Player 1!
            player1.InputBoard.ClearBehaviors();
            player1.InputBoard.AddBehavior(new PressStartToContinueSoulcaliburIIIBehavior(GameEmuHandle));
            player1.InputBoard.AddBehavior(new FileInterpretedBehavior(BehaviorTextBox.Text, GameEmuHandle));

            // Views
            StartPauseButton.Text = "Pause";
            SpeedMultiplierTextBox.Enabled = false;
            SetForegroundWindow(GameEmuHandle);

            // Input threads
            player1.Play(GameEmuHandle, this, true);
            
            if(TwoPlayersMode)
            {
                player2.Play(GameEmuHandle, this, false);
            }
            
        }

        public Thread CreateThread(IntPtr emuHandle, Hand hand, bool main, InputBoard inputBoard) {
            return new Thread(() =>
            {
                bool leftSide = hand == Hand.Left;
                Label label = leftSide ? LeftInputLabel : RightInputLabel;

                Thread.CurrentThread.IsBackground = true;

                // Vars
                int multiplier = Utils.IntFromTextBox(SpeedMultiplierTextBox, 1);
                int minHoldTime = Utils.IntFromTextBox(leftSide ? HoldTimeLeftFrom : HoldTimeRightFrom, 50);
                int maxHoldTime = Utils.IntFromTextBox(leftSide ? HoldTimeLeftTo : HoldTimeRightTo, 50);
                int sleepTime = Utils.IntFromTextBox(leftSide? SleepTimeLeftTextBox : SleepTimeRightTextBox, 0);
                long startGameMS = Utils.parseHourToMilliseconds(FastClockTextBox);
                long startRealMS = Utils.parseHourToMilliseconds(NormalClockTextBox);
                long startWorldMS = Utils.millisecondsSinceEpoch();

                int continues = Utils.IntFromTextBox(continuesTextBox, 0);
                int arcades = Utils.IntFromTextBox(arcadeTextBox, 0);
                
                while (true)
                {
                    if (Utils.GetWindowCaption(emuHandle).Equals(Utils.GetWindowCaption(GetForegroundWindow())))
                    {

                        if(main) {
                            bool somethingHappened = inputBoard.ExecAdditionalBehaviors();
                            if (somethingHappened) continue;
                        }

                        string[] nextCombo = inputBoard.NextCombo(hand);

                        Invoke(new Action(() =>
                        {
                            string inputName = inputBoard.ComboName(nextCombo);
                            label.Text = inputName;

                            if(!string.IsNullOrEmpty(inputName)) {
                                HistoryListBox.Items.Add(inputName);
                                int visibleItems = HistoryListBox.ClientSize.Height / HistoryListBox.ItemHeight;
                                HistoryListBox.TopIndex = Math.Max(HistoryListBox.Items.Count - visibleItems + 1, 0);
                            }

                            if (main) { 
                                long secondsElapsed = Utils.millisecondsSinceEpoch() - startWorldMS;
                                Utils.fillWithDate(NormalClockTextBox, secondsElapsed + startRealMS);
                                Utils.fillWithDate(FastClockTextBox, secondsElapsed * multiplier + startGameMS);
                            }

                            StartAnArcadeBehavior arcadeBehavior = inputBoard.getBehavior<StartAnArcadeBehavior>();
                            if (arcadeBehavior != null) {
                                arcadeTextBox.Text = (arcadeBehavior.ArcadeStarted + arcades).ToString();
                            }

                            PressStartToContinueSoulcaliburIIIBehavior continueBehavior = inputBoard.getBehavior<PressStartToContinueSoulcaliburIIIBehavior>();
                            if (continueBehavior != null)
                            {
                                continuesTextBox.Text = (continueBehavior.Continues + continues).ToString();
                            }

                        }));
                        

                        if(!Behavior.IsLocked()) { 
                           // inputBoard.PressCombo(nextCombo, minHoldTime, maxHoldTime);
                        }

                        Thread.Sleep(sleepTime);
                    }
                    else
                    {
                        if (!Text.Equals(Utils.GetWindowCaption(GetForegroundWindow())))
                        {
                            SetForegroundWindow(emuHandle);
                        }
                        else
                        {
                            Pause();
                        }
                    }
                }

            });
        }

        private void Pause()
        {
            Invoke(new Action(() =>
            {
                SpeedMultiplierTextBox.Enabled = true;

                LeftInputLabel.Text = "-Paused-";
                StartPauseButton.Text = "Start";


                player1.Pause();
                player2.Pause();

            }));
        }
        
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (player1 != null)
                    player1.InputBoard.Enable((string)AllowedInputListBox.Items[e.Index]);
                if(player2 != null)
                    player2.InputBoard.Enable((string)AllowedInputListBox.Items[e.Index]);
            }
            else
            {
                if (player1 != null)
                    player1.InputBoard.Disable((string)AllowedInputListBox.Items[e.Index]);
                if (player2 != null)
                    player2.InputBoard.Disable((string)AllowedInputListBox.Items[e.Index]);
            }

        }

        private void ComboBoxRefreshButton_Click(object sender, EventArgs e)
        {
            RefreshComboBox();
        }

       

        private void SaveStartScreenButton_Click(object sender, EventArgs e)
        {
            //IntPtr emuHandle = FindWindow(null, EmuWindowTitle);
            SetForegroundWindow(GameEmuHandle);
            ScreenshotManager.SaveScreenAs(ScreenshotManager.GenerateStartScreenPath(), GameEmuHandle);
        }
        
        private void RefreshComboBox()
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

            WindowNameDropDown.Items.Add(" - Processes - ");


            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.ProcessName))
                {
                    WindowNameDropDown.Items.Add(process.ProcessName);
                }
            }
        }

        private void TwoHandsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (player1 != null)
                player1.InputBoard.TwoHandMode = TwoHandsCheckBox.Checked;
            if (player2 != null)
                player2.InputBoard.TwoHandMode = TwoHandsCheckBox.Checked;
            setTwoHandLabels();
        }

        private void setTwoHandLabels() {
            if (player1.InputBoard.TwoHandMode)
            {
                LeftHandGroupBox.Text = "Left Hand";
                RightHandGroupBox.Enabled = true;
                LeftInputLabel.Width = RightInputLabel.Width;
                RightInputLabel.Visible = true;
                PlusLabel.Visible = true;
            }
            else
            {
                LeftHandGroupBox.Text = "Main Hand";
                RightHandGroupBox.Enabled = false;
                LeftInputLabel.Width = OriginalLeftInputLabelWidth;
                RightInputLabel.Visible = false;
                PlusLabel.Visible = false;
            }
        }
        
        private void ClearStartScreenButton_Click(object sender, EventArgs e)
        {
            ScreenshotManager.CleanStartScreens();
        }


        // Colors managment

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
            setBackColor(colorDialog1.Color, this, FastClockTextBox, NormalClockTextBox, arcadeTextBox, continuesTextBox);
        }

        private void setForeColor(Color color)
        {
             setForeColor(color, FastClockTextBox, NormalClockTextBox, arcadeTextBox, continuesTextBox, label1, label2, label3, label4, label5, label6, label7, label8,
                PlusLabel, LeftInputLabel, RightInputLabel, groupBox1, groupBox2, LeftHandGroupBox, RightHandGroupBox);
        }

        private void setForeColor(Color color, params Control[] views) {

            foreach (Control control in views) {
                control.ForeColor = colorDialog1.Color;
            }

            foreach (Control control in new Control[]{ ComboBoxRefreshButton, SaveStartScreenButton, ClearStartScreenButton, backColorButton, textColorButton })
            {
                control.ForeColor = Color.Black;
            }
        }

        private void setBackColor(Color color, params Control[] views)
        {
            foreach (Control control in views)
            {
                control.BackColor = colorDialog1.Color;
            }
        }

        private void Controller1Browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Filter = "Comma Separated Values (*.csv)|*.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Controller1MappingTextBox.Text = openFileDialog1.FileName;
                InitPlayers();
            }
        }

        private void Controller2Browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Filter = "Comma Separated Values (*.csv)|*.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Controller2MappingTextBox.Text = openFileDialog1.FileName;
                InitPlayers();
            }
        }

        private void BehaviorBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Filter = "BulletHellFish Behaviors (*.bhf)|*.bhf";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BehaviorTextBox.Text = openFileDialog1.FileName;
                InitPlayers();
            }
        }
    }
}
