using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulletHellFish
{
    class PlayerFormControls
    {

        Player player;

        Label LeftCurrentInput;
        Label RightCurrentInput;
        Label PlusInputLabel;
        public ListBox HistoryListBox;
        public CheckedListBox AllowedListBox;

        GroupBox LeftHandGroupBox;
        TextBox LeftSleepTime;
        TextBox LeftMinHoTime;
        TextBox LeftMaxHoTime;
        CheckBox LeftHandActive;

        GroupBox RightHandGroupBox;
        TextBox RightSleepTime;
        TextBox RightMinHoTime;
        TextBox RightMaxHoTime;
        CheckBox RightHandActive;

        TextBox MappingTextBox;
        TextBox BehaviorTextBox;
        Button MappingBrowseButton;
        Button BehaviorBrowseButton;

        TextBox InputSourceTextBox;
        NumericUpDown InputSourceColumn;
        Button InputSourceBrowseButton;

        Button CopyButton;

        int OriginalLeftInputLabelWidth = 0;
        OpenFileDialog OpenFileDialog;

        BulletHellFishForm Form;

        public PlayerFormControls(BulletHellFishForm Form, Control ParentControl, OpenFileDialog OpenFileDialog)
        {
            FillFromParent(ParentControl);
            this.OpenFileDialog = OpenFileDialog;
            this.Form = Form;
        }

        private void FillFromParent(Control parentControl)
        {
            // Bind Views
            LeftCurrentInput = (Label)parentControl.GetSonTagged("LeftCurrentInput");
            RightCurrentInput = (Label)parentControl.GetSonTagged("RightCurrentInput");
            PlusInputLabel = (Label)parentControl.GetSonTagged("PlusInputLabel");
            HistoryListBox = (ListBox)parentControl.GetSonTagged("HistoryListBox");
            AllowedListBox = (CheckedListBox)parentControl.GetSonTagged("AllowedListBox");

            LeftSleepTime = (TextBox)parentControl.GetSonTagged("LeftSleepTime");
            LeftMinHoTime = (TextBox)parentControl.GetSonTagged("LeftMinHoTime");
            LeftMaxHoTime = (TextBox)parentControl.GetSonTagged("LeftMaxHoTime");
            LeftHandActive = (CheckBox)parentControl.GetSonTagged("LeftHandActive");
            LeftHandGroupBox = (GroupBox)parentControl.GetSonTagged("LeftHandGroupBox");

            RightSleepTime = (TextBox)parentControl.GetSonTagged("RightSleepTime");
            RightMinHoTime = (TextBox)parentControl.GetSonTagged("RightMinHoTime");
            RightMaxHoTime = (TextBox)parentControl.GetSonTagged("RightMaxHoTime");
            RightHandActive = (CheckBox)parentControl.GetSonTagged("RightHandActive");
            RightHandGroupBox = (GroupBox)parentControl.GetSonTagged("RightHandGroupBox");

            MappingTextBox = (TextBox)parentControl.GetSonTagged("MappingTextBox");
            BehaviorTextBox = (TextBox)parentControl.GetSonTagged("BehaviorTextBox");

            MappingBrowseButton = (Button)parentControl.GetSonTagged("MappingBrowseButton");
            BehaviorBrowseButton = (Button)parentControl.GetSonTagged("BehaviorBrowseButton");

            InputSourceTextBox = (TextBox)parentControl.GetSonTagged("InputSourceTextBox");
            InputSourceColumn = (NumericUpDown)parentControl.GetSonTagged("InputSourceColumn");
            InputSourceBrowseButton = (Button)parentControl.GetSonTagged("InputSourceBrowseButton");

            CopyButton = (Button)parentControl.GetSonTagged("CopyButton");

            // Standard Values
            OriginalLeftInputLabelWidth = LeftCurrentInput.Width;

            // Link Events
            RightHandActive.CheckedChanged += new EventHandler(this.OnRightHandActiveChanged);
            AllowedListBox.ItemCheck += new ItemCheckEventHandler(this.OnAllowedItemChecked);
            MappingBrowseButton.Click += new System.EventHandler(this.OnMappingBrowseClick);
            BehaviorBrowseButton.Click += new System.EventHandler(this.OnBehaviorsBrowseClick);
            InputSourceBrowseButton.Click += new System.EventHandler(this.OnInputSourceBrowseClick);

            InputSourceColumn.ValueChanged += new System.EventHandler(this.OnInputSourceColumnValueChanged);
            InputSourceTextBox.TextChanged += new System.EventHandler(this.OnInputSourceTextValueChanged);
        }

        internal void SetPlayer(Player player)
        {
            this.player = player;
        }

        internal string GetMappingPath()
        {
            return MappingTextBox.Text;
        }

        internal bool HasMapping()
        {
            return !string.IsNullOrWhiteSpace(GetMappingPath());
        }

        internal void ClearAllowedInputs()
        {
            AllowedListBox.Items.Clear();
        }

        internal void AddAllowedInput(string input)
        {
            AllowedListBox.Items.Add(input);
            AllowedListBox.SetItemChecked(AllowedListBox.Items.IndexOf(input), true);
        }
        
        public void InitPlayer(Player player)
        {
            SetPlayer(player);

            if (HasMapping())
            {
                player.SetMapping(new PSXInputBoard(GetMappingPath()));
            }

            // Create CheckedListbox
            ClearAllowedInputs();
            foreach (string input in player.InputBoard.EveryInput())
            {
                AddAllowedInput(input);
            }

            OnRightHandActiveChanged(null, null);
        }


        #region events

        private void OnRightHandActiveChanged(object sender, EventArgs e)
        {
            if (player != null)
                player.InputBoard.TwoHandMode = RightHandActive.Checked;
            setTwoHandLabels();
        }


        private void OnAllowedItemChecked(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (player != null)
                    player.InputBoard.Enable((string)AllowedListBox.Items[e.Index]);
            }
            else
            {
                if (player != null)
                    player.InputBoard.Disable((string)AllowedListBox.Items[e.Index]);
            }

        }


        private void OnInputSourceTextValueChanged(object sender, EventArgs e)
        {
            Form.LoadChart();
        }

        private void OnInputSourceColumnValueChanged(object sender, EventArgs e)
        {
            Form.LoadChart();
        }

        private void OnInputSourceBrowseClick(object sender, EventArgs e)
        {
            if (OnFileSelected("Comma Separated Values (*.csv)|*.csv"))
            {
                InputSourceTextBox.Text = OpenFileDialog.FileName;
                Form.LoadChart();
            }

        }

        private void OnMappingBrowseClick(object sender, EventArgs e)
        {
            if (OnFileSelected("Comma Separated Values (*.csv)|*.csv"))
            {
                MappingTextBox.Text = OpenFileDialog.FileName;
                InitPlayer(player);
            }
        }



        private void OnBehaviorsBrowseClick(object sender, EventArgs e)
        {
            if (OnFileSelected("BulletHellFish Behaviors (*.bhf)|*.bhf"))
            {
                BehaviorTextBox.Text = OpenFileDialog.FileName;
                InitPlayer(player);
            }
        }


        private bool OnFileSelected(string filter)
        {
            OpenFileDialog.Multiselect = false;
            OpenFileDialog.CheckFileExists = true;
            OpenFileDialog.Filter = filter;
            string CombinedPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..");
            OpenFileDialog.InitialDirectory = CombinedPath;

            return OpenFileDialog.ShowDialog() == DialogResult.OK;
       }

        #endregion

        private void setTwoHandLabels()
        {
            if (player.InputBoard.TwoHandMode)
            {
                LeftHandGroupBox.Text = "Left Hand";
                RightHandGroupBox.Enabled = true;
                LeftCurrentInput.Width = RightCurrentInput.Width;
                RightCurrentInput.Visible = true;
                PlusInputLabel.Visible = true;
            }
            else
            {
                LeftHandGroupBox.Text = "Main Hand";
                RightHandGroupBox.Enabled = false;
                LeftCurrentInput.Width = OriginalLeftInputLabelWidth;
                RightCurrentInput.Visible = false;
                PlusInputLabel.Visible = false;
            }
        }


        internal void LoadBehaviors(IntPtr GameWindowHandle, string saveScreenShotPath)
        {
            player.InputBoard.ClearBehaviors();

            string path = BehaviorTextBox.Text;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string[] behaviorsPaths = new string[] { };
                if (File.Exists(path))
                {
                    behaviorsPaths = new string[]{ path };
                }
                else if (Directory.Exists(path))
                {
                    behaviorsPaths = Directory.GetFiles(path, "*.bhf");
                } else {
                    MessageBox.Show("Behavior not found", "Behavior file or directory '" + path + "' not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                foreach(string behaviorPath in behaviorsPaths) { 
                    Behavior behavior = new FileInterpretedBehavior(behaviorPath, GameWindowHandle);
                    behavior.SetFolder(saveScreenShotPath);
                    player.InputBoard.AddBehavior(behavior);
                }
            }
        }

        internal Label GetInputLabel(Hand hand)
        {
            return hand == Hand.Left ? LeftCurrentInput : RightCurrentInput;
        }

        internal List<double> GetSource()
        {
            return DataFromFile(InputSourceTextBox.Text, (int)InputSourceColumn.Value);
        }


        public List<double> DataFromFile(string path, int column)
        {
            List<double> doubles = new List<double>();

            if (!System.IO.File.Exists(path)) return doubles;

            foreach (string line in System.IO.File.ReadAllLines(path))
            {
                string[] fields = line.Split(';');
                if (fields.Length < 1) continue;
                if (fields.Length <= column) column = 0;

                double d;
                if (Double.TryParse(fields[column], out d))
                {
                    doubles.Add(d);
                }

            }

            return doubles;
        }
    }
}
