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

        public Player(InputBoard InputBoard, int playerId)
        {
            this.InputBoard = InputBoard;
            this.playerId = playerId;
        }

        public void Play(IntPtr emuHandle, BulletHellFishForm form, bool main)
        {
            leftHandThread = CreateThread(emuHandle, Hand.Left, main, InputBoard, form);
            leftHandThread.Start();
            if (InputBoard.TwoHandMode)
            {
                rightHandThread = CreateThread(emuHandle, Hand.Right, false, InputBoard, form);
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



        public Thread CreateThread(IntPtr emuHandle, Hand hand, bool main, InputBoard inputBoard, BulletHellFishForm form)
        {
            return new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                InputParameters param = form.GetInputParameters(hand);

                while (true)
                {
                    if (Utils.GetWindowCaption(emuHandle).Equals(Utils.GetWindowCaption(GetForegroundWindow())))
                    {

                        if (main)
                        {
                            bool somethingHappened = inputBoard.ExecAdditionalBehaviors();
                            if (somethingHappened) continue;
                        }

                        string[] nextCombo = inputBoard.NextCombo(hand);

                        if (!Behavior.IsLocked())
                        {
                            inputBoard.PressCombo(nextCombo, param.MinHoldTime, param.MaxHoldTime);
                        }
                        else
                        {
                            continue;
                        }

                        form.UpdateForm(inputBoard.ComboName(nextCombo), hand, main, playerId);

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
    }
}
