﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public abstract class Behavior
    {
        protected InputBoard inputBoard;
        protected int tick = 0;
        protected int waitCycle = 0;

        protected Behavior(int waitCycle)
        {
            this.waitCycle = waitCycle;
        }

        public abstract bool behave();

        protected bool skip() {

            tick += 1;
            if (tick >= waitCycle)
            {
                tick = 0;
                return false;
            }
            return true;

        }

        internal void SetInputBoard(InputBoard inputBoard)
        {
            this.inputBoard = inputBoard;
        }
    }

    public class PressStartAtTitleScreenBehavior : Behavior
    {
        protected IntPtr emuHandle;

        public PressStartAtTitleScreenBehavior(IntPtr emuHandle) : base(10)
        {
            this.emuHandle = emuHandle;
        }
        
        public override bool behave() {
            if (skip()) return false;

            if (ScreenshotManager.StartScreenPresent(emuHandle))
            {
                inputBoard.PressStart();
                return true;
            }
            return false;
        }
    }

    public class PressStartToContinueBehavior : Behavior
    {
        protected IntPtr emuHandle;
        private bool lastFrameWasContinue = false;
        public int Continues = 0;

        public PressStartToContinueBehavior(IntPtr emuHandle) : base(0)
        {
            this.emuHandle = emuHandle;
        }

        public override bool behave()
        {
            if (ScreenshotManager.ThisScreenPresent("continue_screen.jpg", emuHandle, new System.Drawing.Rectangle( 117 , 229 , 343 , 35)))
            {
                if (!lastFrameWasContinue) {
                    Continues++;
                }
                inputBoard.PressStart();
                lastFrameWasContinue = true;
                return true;
            }
            lastFrameWasContinue = false;
            return false;
        }
    }

    public class StartAnArcadeBehavior : Behavior
    {
        protected IntPtr emuHandle;
        private bool lastFrameWasArcade = false;
        public int ArcadeStarted = 0;

        public StartAnArcadeBehavior(IntPtr emuHandle) : base(0)
        {
            this.emuHandle = emuHandle;
        }

        public override bool behave() {
            bool isTitleScreen = ScreenshotManager.ThisScreenPresent("title_screen.jpg", emuHandle,  0.95);
            bool isSelectScreen = ScreenshotManager.ThisScreenPresent("select_screen.jpg", emuHandle, 0.75);


            if (isTitleScreen)
            {
                if (lastFrameWasArcade) {
                    return true;
                }
                lastFrameWasArcade = true;
                inputBoard.PressStart();
                ArcadeStarted++;

                int tries = 3;
                do
                {
                    tries--;
                    Thread.Sleep(1000);
                    isSelectScreen = ScreenshotManager.ThisScreenPresent("select_screen.jpg", emuHandle);
                } while (!isSelectScreen && tries > 0);

                if (tries <= 0) {
                    //Something went wrong, just forget about the tart a new arcade thingy
                    ArcadeStarted--;
                    return false;
                }

                Random r = new Random(DateTime.Now.Millisecond);
                int hitLeft = r.Next(0, 10);
                for (int i = 0; i < hitLeft; i++)
                {
                    inputBoard.Press(InputBoard.RIGHT, 150, 150);
                    Thread.Sleep(100);

                }

                inputBoard.PressConfirm();
                return true;
            }
            else if (isSelectScreen) {
                return true;
            }
            lastFrameWasArcade = false;
            return false;
        }
    }
}
