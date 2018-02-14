using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BulletHellFish
{
    class Player
    {
        
        private Thread leftHandThread;
        private Thread rightHandThread;
        public InputBoard InputBoard;

        public Player(InputBoard inputBoard)
        {
            this.InputBoard = inputBoard;
        }

        public void Play(IntPtr emuHandle, BulletHellFishForm form, bool main)
        {
            leftHandThread = form.CreateThread(emuHandle, Hand.Left, main, InputBoard);
            leftHandThread.Start();
            if (InputBoard.TwoHandMode)
            {
                rightHandThread = form.CreateThread(emuHandle, Hand.Right, false, InputBoard);
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
    }
}
