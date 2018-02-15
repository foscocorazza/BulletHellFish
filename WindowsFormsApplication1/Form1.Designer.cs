namespace BulletHellFish
{
    partial class BulletHellFishForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartPauseButton = new System.Windows.Forms.Button();
            this.LeftInputLabel = new System.Windows.Forms.Label();
            this.HistoryListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SleepTimeLeftTextBox = new System.Windows.Forms.TextBox();
            this.AllowedInputListBox = new System.Windows.Forms.CheckedListBox();
            this.NormalClockTextBox = new System.Windows.Forms.TextBox();
            this.FastClockTextBox = new System.Windows.Forms.TextBox();
            this.SpeedMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.HoldTimeLeftFrom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.HoldTimeLeftTo = new System.Windows.Forms.TextBox();
            this.WindowNameDropDown = new System.Windows.Forms.ComboBox();
            this.ComboBoxRefreshButton = new System.Windows.Forms.Button();
            this.TwoHandsCheckBox = new System.Windows.Forms.CheckBox();
            this.RightInputLabel = new System.Windows.Forms.Label();
            this.PlusLabel = new System.Windows.Forms.Label();
            this.SaveStartScreenButton = new System.Windows.Forms.Button();
            this.LeftHandGroupBox = new System.Windows.Forms.GroupBox();
            this.RightHandGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SleepTimeRightTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.HoldTimeRightFrom = new System.Windows.Forms.TextBox();
            this.HoldTimeRightTo = new System.Windows.Forms.TextBox();
            this.ClearStartScreenButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textColorButton = new System.Windows.Forms.Button();
            this.backColorButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.arcadeTextBox = new System.Windows.Forms.TextBox();
            this.continuesTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.Controller1MappingTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Controller2MappingTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.BehaviorTextBox = new System.Windows.Forms.TextBox();
            this.Controller1Browse = new System.Windows.Forms.Button();
            this.Controller2Browse = new System.Windows.Forms.Button();
            this.BehaviorBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LeftHandGroupBox.SuspendLayout();
            this.RightHandGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartPauseButton
            // 
            this.StartPauseButton.Location = new System.Drawing.Point(13, 291);
            this.StartPauseButton.Name = "StartPauseButton";
            this.StartPauseButton.Size = new System.Drawing.Size(278, 23);
            this.StartPauseButton.TabIndex = 0;
            this.StartPauseButton.Text = "Start";
            this.StartPauseButton.UseVisualStyleBackColor = true;
            this.StartPauseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // LeftInputLabel
            // 
            this.LeftInputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeftInputLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.LeftInputLabel.Location = new System.Drawing.Point(12, 31);
            this.LeftInputLabel.Name = "LeftInputLabel";
            this.LeftInputLabel.Size = new System.Drawing.Size(279, 68);
            this.LeftInputLabel.TabIndex = 1;
            this.LeftInputLabel.Text = "Left Input";
            this.LeftInputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HistoryListBox
            // 
            this.HistoryListBox.FormattingEnabled = true;
            this.HistoryListBox.IntegralHeight = false;
            this.HistoryListBox.Location = new System.Drawing.Point(13, 111);
            this.HistoryListBox.Name = "HistoryListBox";
            this.HistoryListBox.Size = new System.Drawing.Size(131, 139);
            this.HistoryListBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "SleepTime (ms)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SleepTimeLeftTextBox
            // 
            this.SleepTimeLeftTextBox.Location = new System.Drawing.Point(101, 19);
            this.SleepTimeLeftTextBox.Name = "SleepTimeLeftTextBox";
            this.SleepTimeLeftTextBox.Size = new System.Drawing.Size(101, 20);
            this.SleepTimeLeftTextBox.TabIndex = 6;
            this.SleepTimeLeftTextBox.Text = "0";
            this.SleepTimeLeftTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AllowedInputListBox
            // 
            this.AllowedInputListBox.CheckOnClick = true;
            this.AllowedInputListBox.FormattingEnabled = true;
            this.AllowedInputListBox.Location = new System.Drawing.Point(150, 111);
            this.AllowedInputListBox.Name = "AllowedInputListBox";
            this.AllowedInputListBox.Size = new System.Drawing.Size(141, 139);
            this.AllowedInputListBox.TabIndex = 8;
            this.AllowedInputListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // NormalClockTextBox
            // 
            this.NormalClockTextBox.BackColor = System.Drawing.Color.Maroon;
            this.NormalClockTextBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NormalClockTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NormalClockTextBox.Location = new System.Drawing.Point(150, 259);
            this.NormalClockTextBox.Name = "NormalClockTextBox";
            this.NormalClockTextBox.Size = new System.Drawing.Size(141, 26);
            this.NormalClockTextBox.TabIndex = 9;
            this.NormalClockTextBox.Text = "00:00:00";
            this.NormalClockTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FastClockTextBox
            // 
            this.FastClockTextBox.BackColor = System.Drawing.Color.Maroon;
            this.FastClockTextBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FastClockTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FastClockTextBox.Location = new System.Drawing.Point(13, 259);
            this.FastClockTextBox.Name = "FastClockTextBox";
            this.FastClockTextBox.Size = new System.Drawing.Size(131, 26);
            this.FastClockTextBox.TabIndex = 10;
            this.FastClockTextBox.Text = "00:00:00";
            this.FastClockTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SpeedMultiplierTextBox
            // 
            this.SpeedMultiplierTextBox.Location = new System.Drawing.Point(57, 255);
            this.SpeedMultiplierTextBox.Name = "SpeedMultiplierTextBox";
            this.SpeedMultiplierTextBox.Size = new System.Drawing.Size(49, 20);
            this.SpeedMultiplierTextBox.TabIndex = 11;
            this.SpeedMultiplierTextBox.Text = "1";
            this.SpeedMultiplierTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Speed";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(14, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Window Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HoldTimeLeftFrom
            // 
            this.HoldTimeLeftFrom.Location = new System.Drawing.Point(101, 45);
            this.HoldTimeLeftFrom.Name = "HoldTimeLeftFrom";
            this.HoldTimeLeftFrom.Size = new System.Drawing.Size(50, 20);
            this.HoldTimeLeftFrom.TabIndex = 18;
            this.HoldTimeLeftFrom.Text = "50";
            this.HoldTimeLeftFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Hold Time (ms)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HoldTimeLeftTo
            // 
            this.HoldTimeLeftTo.Location = new System.Drawing.Point(152, 45);
            this.HoldTimeLeftTo.Name = "HoldTimeLeftTo";
            this.HoldTimeLeftTo.Size = new System.Drawing.Size(50, 20);
            this.HoldTimeLeftTo.TabIndex = 19;
            this.HoldTimeLeftTo.Text = "1000";
            this.HoldTimeLeftTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // WindowNameDropDown
            // 
            this.WindowNameDropDown.FormattingEnabled = true;
            this.WindowNameDropDown.Location = new System.Drawing.Point(107, 6);
            this.WindowNameDropDown.Name = "WindowNameDropDown";
            this.WindowNameDropDown.Size = new System.Drawing.Size(184, 21);
            this.WindowNameDropDown.TabIndex = 20;
            // 
            // ComboBoxRefreshButton
            // 
            this.ComboBoxRefreshButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ComboBoxRefreshButton.Location = new System.Drawing.Point(15, 19);
            this.ComboBoxRefreshButton.Name = "ComboBoxRefreshButton";
            this.ComboBoxRefreshButton.Size = new System.Drawing.Size(208, 23);
            this.ComboBoxRefreshButton.TabIndex = 21;
            this.ComboBoxRefreshButton.Text = "Reload windows names";
            this.ComboBoxRefreshButton.UseVisualStyleBackColor = true;
            this.ComboBoxRefreshButton.Click += new System.EventHandler(this.ComboBoxRefreshButton_Click);
            // 
            // TwoHandsCheckBox
            // 
            this.TwoHandsCheckBox.AutoSize = true;
            this.TwoHandsCheckBox.Checked = true;
            this.TwoHandsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TwoHandsCheckBox.Location = new System.Drawing.Point(125, 257);
            this.TwoHandsCheckBox.Name = "TwoHandsCheckBox";
            this.TwoHandsCheckBox.Size = new System.Drawing.Size(95, 17);
            this.TwoHandsCheckBox.TabIndex = 22;
            this.TwoHandsCheckBox.Text = "2 Hands mode";
            this.TwoHandsCheckBox.UseVisualStyleBackColor = true;
            this.TwoHandsCheckBox.CheckedChanged += new System.EventHandler(this.TwoHandsCheckBox_CheckedChanged);
            // 
            // RightInputLabel
            // 
            this.RightInputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RightInputLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.RightInputLabel.Location = new System.Drawing.Point(160, 31);
            this.RightInputLabel.Name = "RightInputLabel";
            this.RightInputLabel.Size = new System.Drawing.Size(131, 68);
            this.RightInputLabel.TabIndex = 23;
            this.RightInputLabel.Text = "⬀⇦ ⇨";
            this.RightInputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlusLabel
            // 
            this.PlusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.PlusLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.PlusLabel.Location = new System.Drawing.Point(133, 33);
            this.PlusLabel.Name = "PlusLabel";
            this.PlusLabel.Size = new System.Drawing.Size(30, 68);
            this.PlusLabel.TabIndex = 24;
            this.PlusLabel.Text = "+";
            this.PlusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SaveStartScreenButton
            // 
            this.SaveStartScreenButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SaveStartScreenButton.Location = new System.Drawing.Point(15, 45);
            this.SaveStartScreenButton.Name = "SaveStartScreenButton";
            this.SaveStartScreenButton.Size = new System.Drawing.Size(208, 23);
            this.SaveStartScreenButton.TabIndex = 25;
            this.SaveStartScreenButton.Text = "Save Start Screen";
            this.SaveStartScreenButton.UseVisualStyleBackColor = true;
            this.SaveStartScreenButton.Click += new System.EventHandler(this.SaveStartScreenButton_Click);
            // 
            // LeftHandGroupBox
            // 
            this.LeftHandGroupBox.Controls.Add(this.label2);
            this.LeftHandGroupBox.Controls.Add(this.SleepTimeLeftTextBox);
            this.LeftHandGroupBox.Controls.Add(this.label7);
            this.LeftHandGroupBox.Controls.Add(this.HoldTimeLeftFrom);
            this.LeftHandGroupBox.Controls.Add(this.HoldTimeLeftTo);
            this.LeftHandGroupBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.LeftHandGroupBox.Location = new System.Drawing.Point(15, 98);
            this.LeftHandGroupBox.Name = "LeftHandGroupBox";
            this.LeftHandGroupBox.Size = new System.Drawing.Size(208, 75);
            this.LeftHandGroupBox.TabIndex = 26;
            this.LeftHandGroupBox.TabStop = false;
            this.LeftHandGroupBox.Text = "Left Hand";
            // 
            // RightHandGroupBox
            // 
            this.RightHandGroupBox.Controls.Add(this.label1);
            this.RightHandGroupBox.Controls.Add(this.SleepTimeRightTextBox);
            this.RightHandGroupBox.Controls.Add(this.label8);
            this.RightHandGroupBox.Controls.Add(this.HoldTimeRightFrom);
            this.RightHandGroupBox.Controls.Add(this.HoldTimeRightTo);
            this.RightHandGroupBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.RightHandGroupBox.Location = new System.Drawing.Point(15, 175);
            this.RightHandGroupBox.Name = "RightHandGroupBox";
            this.RightHandGroupBox.Size = new System.Drawing.Size(208, 75);
            this.RightHandGroupBox.TabIndex = 27;
            this.RightHandGroupBox.TabStop = false;
            this.RightHandGroupBox.Text = "Right Hand";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "SleepTime (ms)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SleepTimeRightTextBox
            // 
            this.SleepTimeRightTextBox.Location = new System.Drawing.Point(101, 19);
            this.SleepTimeRightTextBox.Name = "SleepTimeRightTextBox";
            this.SleepTimeRightTextBox.Size = new System.Drawing.Size(101, 20);
            this.SleepTimeRightTextBox.TabIndex = 6;
            this.SleepTimeRightTextBox.Text = "0";
            this.SleepTimeRightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Hold Time (ms)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HoldTimeRightFrom
            // 
            this.HoldTimeRightFrom.Location = new System.Drawing.Point(101, 45);
            this.HoldTimeRightFrom.Name = "HoldTimeRightFrom";
            this.HoldTimeRightFrom.Size = new System.Drawing.Size(50, 20);
            this.HoldTimeRightFrom.TabIndex = 18;
            this.HoldTimeRightFrom.Text = "50";
            this.HoldTimeRightFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HoldTimeRightTo
            // 
            this.HoldTimeRightTo.Location = new System.Drawing.Point(152, 45);
            this.HoldTimeRightTo.Name = "HoldTimeRightTo";
            this.HoldTimeRightTo.Size = new System.Drawing.Size(50, 20);
            this.HoldTimeRightTo.TabIndex = 19;
            this.HoldTimeRightTo.Text = "1000";
            this.HoldTimeRightTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ClearStartScreenButton
            // 
            this.ClearStartScreenButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ClearStartScreenButton.Location = new System.Drawing.Point(15, 71);
            this.ClearStartScreenButton.Name = "ClearStartScreenButton";
            this.ClearStartScreenButton.Size = new System.Drawing.Size(208, 23);
            this.ClearStartScreenButton.TabIndex = 28;
            this.ClearStartScreenButton.Text = "Clear Start Screen";
            this.ClearStartScreenButton.UseVisualStyleBackColor = true;
            this.ClearStartScreenButton.Click += new System.EventHandler(this.ClearStartScreenButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textColorButton);
            this.groupBox1.Controls.Add(this.backColorButton);
            this.groupBox1.Controls.Add(this.TwoHandsCheckBox);
            this.groupBox1.Controls.Add(this.ComboBoxRefreshButton);
            this.groupBox1.Controls.Add(this.ClearStartScreenButton);
            this.groupBox1.Controls.Add(this.SpeedMultiplierTextBox);
            this.groupBox1.Controls.Add(this.RightHandGroupBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.LeftHandGroupBox);
            this.groupBox1.Controls.Add(this.SaveStartScreenButton);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(307, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 308);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced";
            // 
            // textColorButton
            // 
            this.textColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textColorButton.Location = new System.Drawing.Point(116, 279);
            this.textColorButton.Name = "textColorButton";
            this.textColorButton.Size = new System.Drawing.Size(107, 23);
            this.textColorButton.TabIndex = 30;
            this.textColorButton.Text = "Textcolor";
            this.textColorButton.UseVisualStyleBackColor = true;
            this.textColorButton.Click += new System.EventHandler(this.textColorButton_Click);
            // 
            // backColorButton
            // 
            this.backColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backColorButton.Location = new System.Drawing.Point(15, 279);
            this.backColorButton.Name = "backColorButton";
            this.backColorButton.Size = new System.Drawing.Size(95, 23);
            this.backColorButton.TabIndex = 29;
            this.backColorButton.Text = "Backcolor";
            this.backColorButton.UseVisualStyleBackColor = true;
            this.backColorButton.Click += new System.EventHandler(this.backColorButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.arcadeTextBox);
            this.groupBox2.Controls.Add(this.continuesTextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(556, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(233, 308);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tekken 3";
            // 
            // arcadeTextBox
            // 
            this.arcadeTextBox.BackColor = System.Drawing.Color.Maroon;
            this.arcadeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.arcadeTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.arcadeTextBox.Location = new System.Drawing.Point(10, 149);
            this.arcadeTextBox.Name = "arcadeTextBox";
            this.arcadeTextBox.Size = new System.Drawing.Size(215, 38);
            this.arcadeTextBox.TabIndex = 34;
            this.arcadeTextBox.Text = "0";
            this.arcadeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // continuesTextBox
            // 
            this.continuesTextBox.BackColor = System.Drawing.Color.Maroon;
            this.continuesTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.continuesTextBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.continuesTextBox.Location = new System.Drawing.Point(12, 45);
            this.continuesTextBox.Name = "continuesTextBox";
            this.continuesTextBox.Size = new System.Drawing.Size(215, 38);
            this.continuesTextBox.TabIndex = 30;
            this.continuesTextBox.Text = "0";
            this.continuesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(6, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(221, 26);
            this.label5.TabIndex = 33;
            this.label5.Text = "Full Arcades";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 26);
            this.label4.TabIndex = 32;
            this.label4.Text = "Continues";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 341);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Controller 1 Mapping";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Controller1MappingTextBox
            // 
            this.Controller1MappingTextBox.Location = new System.Drawing.Point(122, 338);
            this.Controller1MappingTextBox.Name = "Controller1MappingTextBox";
            this.Controller1MappingTextBox.Size = new System.Drawing.Size(145, 20);
            this.Controller1MappingTextBox.TabIndex = 21;
            this.Controller1MappingTextBox.Text = "..\\..\\..\\Input Sheets\\PSX Input Sheet P1.csv";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 367);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Controller 2 Mapping";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Controller2MappingTextBox
            // 
            this.Controller2MappingTextBox.Location = new System.Drawing.Point(122, 364);
            this.Controller2MappingTextBox.Name = "Controller2MappingTextBox";
            this.Controller2MappingTextBox.Size = new System.Drawing.Size(145, 20);
            this.Controller2MappingTextBox.TabIndex = 31;
            this.Controller2MappingTextBox.Text = "..\\..\\..\\Input Sheets\\PSX Input Sheet P2.csv";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 393);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Behavior";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BehaviorTextBox
            // 
            this.BehaviorTextBox.Location = new System.Drawing.Point(122, 390);
            this.BehaviorTextBox.Name = "BehaviorTextBox";
            this.BehaviorTextBox.Size = new System.Drawing.Size(145, 20);
            this.BehaviorTextBox.TabIndex = 33;
            this.BehaviorTextBox.Text = "..\\..\\..\\Behaviors\\Behavior.bhf";
            // 
            // Controller1Browse
            // 
            this.Controller1Browse.Location = new System.Drawing.Point(273, 338);
            this.Controller1Browse.Name = "Controller1Browse";
            this.Controller1Browse.Size = new System.Drawing.Size(54, 20);
            this.Controller1Browse.TabIndex = 34;
            this.Controller1Browse.Text = "Browse";
            this.Controller1Browse.UseVisualStyleBackColor = true;
            this.Controller1Browse.Click += new System.EventHandler(this.Controller1Browse_Click);
            // 
            // Controller2Browse
            // 
            this.Controller2Browse.Location = new System.Drawing.Point(273, 364);
            this.Controller2Browse.Name = "Controller2Browse";
            this.Controller2Browse.Size = new System.Drawing.Size(54, 20);
            this.Controller2Browse.TabIndex = 35;
            this.Controller2Browse.Text = "Browse";
            this.Controller2Browse.UseVisualStyleBackColor = true;
            this.Controller2Browse.Click += new System.EventHandler(this.Controller2Browse_Click);
            // 
            // BehaviorBrowse
            // 
            this.BehaviorBrowse.Location = new System.Drawing.Point(273, 390);
            this.BehaviorBrowse.Name = "BehaviorBrowse";
            this.BehaviorBrowse.Size = new System.Drawing.Size(54, 20);
            this.BehaviorBrowse.TabIndex = 36;
            this.BehaviorBrowse.Text = "Browse";
            this.BehaviorBrowse.UseVisualStyleBackColor = true;
            this.BehaviorBrowse.Click += new System.EventHandler(this.BehaviorBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BulletHellFishForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(804, 464);
            this.Controls.Add(this.BehaviorBrowse);
            this.Controls.Add(this.Controller2Browse);
            this.Controls.Add(this.Controller1Browse);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.BehaviorTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Controller2MappingTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Controller1MappingTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PlusLabel);
            this.Controls.Add(this.RightInputLabel);
            this.Controls.Add(this.WindowNameDropDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FastClockTextBox);
            this.Controls.Add(this.NormalClockTextBox);
            this.Controls.Add(this.AllowedInputListBox);
            this.Controls.Add(this.HistoryListBox);
            this.Controls.Add(this.LeftInputLabel);
            this.Controls.Add(this.StartPauseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "BulletHellFishForm";
            this.Text = "The Bullet Hell Fish";
            this.LeftHandGroupBox.ResumeLayout(false);
            this.LeftHandGroupBox.PerformLayout();
            this.RightHandGroupBox.ResumeLayout(false);
            this.RightHandGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartPauseButton;
        private System.Windows.Forms.Label LeftInputLabel;
        private System.Windows.Forms.ListBox HistoryListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SleepTimeLeftTextBox;
        private System.Windows.Forms.CheckedListBox AllowedInputListBox;
        private System.Windows.Forms.TextBox NormalClockTextBox;
        private System.Windows.Forms.TextBox FastClockTextBox;
        private System.Windows.Forms.TextBox SpeedMultiplierTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox HoldTimeLeftFrom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox HoldTimeLeftTo;
        private System.Windows.Forms.ComboBox WindowNameDropDown;
        private System.Windows.Forms.Button ComboBoxRefreshButton;
        private System.Windows.Forms.CheckBox TwoHandsCheckBox;
        private System.Windows.Forms.Label RightInputLabel;
        private System.Windows.Forms.Label PlusLabel;
        private System.Windows.Forms.Button SaveStartScreenButton;
        private System.Windows.Forms.GroupBox LeftHandGroupBox;
        private System.Windows.Forms.GroupBox RightHandGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SleepTimeRightTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox HoldTimeRightFrom;
        private System.Windows.Forms.TextBox HoldTimeRightTo;
        private System.Windows.Forms.Button ClearStartScreenButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button textColorButton;
        private System.Windows.Forms.Button backColorButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TextBox arcadeTextBox;
        private System.Windows.Forms.TextBox continuesTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Controller1MappingTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Controller2MappingTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox BehaviorTextBox;
        private System.Windows.Forms.Button Controller1Browse;
        private System.Windows.Forms.Button Controller2Browse;
        private System.Windows.Forms.Button BehaviorBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

