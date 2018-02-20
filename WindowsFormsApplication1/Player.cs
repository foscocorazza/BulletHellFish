using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BulletHellFish
{
    class Player
    {
        #region WindowsImports
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        private int playerId;
        private Thread leftHandThread;
        private Thread rightHandThread;
        public InputBoard InputBoard;

        public Player(int playerId)
        {
            this.playerId = playerId;
        }

        public void Play(IntPtr emuHandle, BulletHellFishForm form, bool main, List<double> dataSource)
        {
            leftHandThread = CreateThread(emuHandle, Hand.Left, main, InputBoard, form, dataSource);
            leftHandThread.Start();
            if (InputBoard.TwoHandMode)
            {
                rightHandThread = CreateThread(emuHandle, Hand.Right, false, InputBoard, form, dataSource);
                rightHandThread.Start();
            }
        }


        public bool IsPlaying()
        {
            return leftHandThread != null;
        }

        public void Pause()
        {
            Utils.Abort(ref leftHandThread);
            Utils.Abort(ref rightHandThread);
        }



        public Thread CreateThread(IntPtr emuHandle, Hand hand, bool main, InputBoard inputBoard, BulletHellFishForm form, List<double> dataSource)
        {
            return new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                InputParameters param = form.GetInputParameters(hand);

                bool noDataSource = dataSource == null || dataSource.Count() == 0;

                int minHoldTime = param.MinHoldTime;
                int maxHoldTime = noDataSource ? minHoldTime : param.MaxHoldTime;

                int i = 0;
                while (true)
                {
                    if (Utils.GetWindowCaption(emuHandle).Equals(Utils.GetWindowCaption(GetForegroundWindow())))
                    {
                        
                        if (main)
                        {
                            inputBoard.ExecAdditionalBehaviors();
                        }

                        string[] nextCombo;

                        if (noDataSource)
                        {
                            nextCombo = inputBoard.NextCombo(hand);
                        } else
                        {
                            nextCombo = inputBoard.NextComboFromNormalizedData(hand, dataSource[i]);
                        }

                        if (!Behavior.IsLocked())
                        {
                            inputBoard.PressCombo(nextCombo, minHoldTime, maxHoldTime, hand);
                            form.UpdateForm(inputBoard.ComboName(nextCombo), hand, main, playerId, i);
                            i++;
                            if (i >= dataSource.Count()) i = 0;
                        }
                        else
                        {
                            continue;
                        }


                        Thread.Sleep(param.SleepTime);
                    }
                    else
                    {
                        if (!form.Text.Equals(Utils.GetWindowCaption(GetForegroundWindow())))
                        {
                            SetForegroundWindow(emuHandle);
                        }
                        else
                        {
                            form.Pause();
                        }
                    }
                }

            });
        }

        internal void SetMapping(PSXInputBoard InputBoard)
        {
            this.InputBoard = InputBoard;
        }
    }
}
