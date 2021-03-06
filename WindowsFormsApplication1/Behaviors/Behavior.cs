﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BulletHellFish
{
    public abstract class Behavior
    {
        protected static readonly string EXT = ".png";
        protected InputBoard inputBoard;
        protected int tick = 0;
        protected int waitCycle = 0;
        protected IntPtr emuHandle;


        protected string folder = "./";
        private static bool locked = false;

        protected Behavior(int waitCycle, IntPtr emuHandle)
        {
            this.waitCycle = waitCycle;
            this.emuHandle = emuHandle;
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

        public static void Lock()
        {
            locked = true;
        }

        public static void Unlock()
        {
            locked = false;
        }

        public static bool IsLocked()
        {
            return locked;
        }

        internal void SetInputBoard(InputBoard inputBoard)
        {
            this.inputBoard = inputBoard;
        }
        

        public void Wait(int ms) {
            Thread.Sleep(ms);
        }

        public void Press(string displayName)
        {
            inputBoard.Press(displayName, 200, 200, Hand.Left);
        }


        public bool IsSimilarTo(string path, System.Drawing.Rectangle rectangle) {
            return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle, rectangle);
        }

        public bool IsSimilarTo(string path)
        {
            return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle);
        }

        public bool IsSimilarTo(string path, System.Drawing.Rectangle rectangle, float similarity)
        {
            if(similarity > 0)
                return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle, rectangle, similarity);
            else
                return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle, rectangle);
        }

        public bool IsSimilarTo(string path, float similarity)
        {
            if (similarity > 0)
                return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle, similarity);
            else
                return ScreenshotManager.ThisScreenPresent(folder, path + EXT, emuHandle);
        }

        public void SetFolder(string folder)
        {
            this.folder = folder;
        }
        
    }

  /*  public class PressStartAtTitleScreenBehavior : Behavior
    {
        public PressStartAtTitleScreenBehavior(IntPtr emuHandle) : base(10, emuHandle)
        {}
        
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

    public class PressStartToContinueTekkenIIIBehavior : Behavior
    {
        private bool lastFrameWasContinue = false;
        public int Continues = 0;

        public PressStartToContinueTekkenIIIBehavior(IntPtr emuHandle) : base(0, emuHandle)
        {}

        public override bool behave() 
        {
            if (IsLocked()) return false;

            if (ScreenshotManager.ThisScreenPresent(folder, "continue_screen"+ EXT, emuHandle, new System.Drawing.Rectangle( 117 , 229 , 343 , 35)))
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


    public class PressStartToContinueSoulcaliburIIIBehavior : Behavior
    {
        private bool lastFrameBehaved = false;
        public int Continues = 0;

        public PressStartToContinueSoulcaliburIIIBehavior(IntPtr emuHandle) : base(0, emuHandle)
        {}

        public override bool behave()
        {
            if (IsLocked()) return false;

            bool VictoryScreen = ScreenshotManager.ThisScreenPresent(folder, "victory_screen"+ EXT, emuHandle, new System.Drawing.Rectangle(77, 620, 1253, 116));
            bool DrawScreen = ScreenshotManager.ThisScreenPresent(folder, "draw_screen" + EXT, emuHandle, new System.Drawing.Rectangle(510, 624, 383, 106));

            if (VictoryScreen || DrawScreen)
            {

                Lock();

                if (!lastFrameBehaved)
                {
                    Continues++;
                }

                Press("Start");

                Wait(1000);

                Press("X");

                Unlock();

                lastFrameBehaved = true;
                return true;
            }
            lastFrameBehaved = false;
            return false;
        }

    }

    public class StartAnArcadeBehavior : Behavior
    {
        private bool lastFrameWasArcade = false;
        public int ArcadeStarted = 0;

        public StartAnArcadeBehavior(IntPtr emuHandle) : base(0, emuHandle)
        {
            this.emuHandle = emuHandle;
        }

        public override bool behave()
        {
            if (IsLocked()) return false;

            bool isTitleScreen = ScreenshotManager.ThisScreenPresent(folder, "title_screen"+ EXT, emuHandle,  0.95);
            bool isSelectScreen = ScreenshotManager.ThisScreenPresent(folder, "select_screen"+ EXT, emuHandle, 0.75);


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
                    isSelectScreen = ScreenshotManager.ThisScreenPresent(folder, "select_screen"+ EXT, emuHandle);
                } while (!isSelectScreen && tries > 0);

                if (tries <= 0) {
                    //Something went wrong, just forget about the start a new arcade thingy
                    ArcadeStarted--;
                    return false;
                }

                Random r = new Random(DateTime.Now.Millisecond);
                int hitLeft = r.Next(0, 10);
                for (int i = 0; i < hitLeft; i++)
                {
                    inputBoard.Press(InputBoard.RIGHT, 150, 150, Hand.Left);
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
    }*/

    public class FileInterpretedBehavior : Behavior
    {

        string[] program;
        BehaviorInterpreter interpreter;

        public FileInterpretedBehavior(string filePath, IntPtr emuHandle) : base(0, emuHandle)
        {
            program = File.ReadAllLines(filePath);
            interpreter = new BehaviorInterpreter(this, program);
        }

        public override bool behave()
        {
            if (IsLocked()) return false;
            return interpreter.Execute();

        }
    }
}
