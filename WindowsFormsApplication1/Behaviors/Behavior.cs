using System;
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
        protected InputBoard inputBoard;
        protected int tick = 0;
        protected int waitCycle = 0;
        protected IntPtr emuHandle;

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
            inputBoard.Press(displayName, 200, 200);
        }


        public bool IsSimilarTo(string path, System.Drawing.Rectangle rectangle) {
            return ScreenshotManager.ThisScreenPresent(path+".jpg", emuHandle, rectangle);
        }

        public bool IsSimilarTo(string path)
        {
            return ScreenshotManager.ThisScreenPresent(path + ".jpg", emuHandle);
        }

        public bool IsSimilarTo(string path, System.Drawing.Rectangle rectangle, float similarity)
        {
            return ScreenshotManager.ThisScreenPresent(path + ".jpg", emuHandle, rectangle, similarity);
        }

        public bool IsSimilarTo(string path, float similarity)
        {
            return ScreenshotManager.ThisScreenPresent(path + ".jpg", emuHandle, similarity);
        }
        
    }

    public class PressStartAtTitleScreenBehavior : Behavior
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


    public class PressStartToContinueSoulcaliburIIIBehavior : Behavior
    {
        private bool lastFrameBehaved = false;
        public int Continues = 0;

        public PressStartToContinueSoulcaliburIIIBehavior(IntPtr emuHandle) : base(0, emuHandle)
        {}

        public override bool behave()
        {
            if (IsLocked()) return false;

            bool VictoryScreen = ScreenshotManager.ThisScreenPresent("victory_screen.jpg", emuHandle, new System.Drawing.Rectangle(77, 620, 1253, 116));
            bool DrawScreen = ScreenshotManager.ThisScreenPresent("draw_screen.jpg", emuHandle, new System.Drawing.Rectangle(510, 624, 383, 106));

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
                    //Something went wrong, just forget about the start a new arcade thingy
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
