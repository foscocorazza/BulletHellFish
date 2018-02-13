using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace WindowsFormsApplication1
{

    public enum Hand
    {
        Left, Right, Any
    }

   public abstract class InputBoard
    {

        protected Dictionary<string, VirtualKeyCode> keys = new Dictionary<string, VirtualKeyCode>();
        private HashSet<Behavior> behaviors = new HashSet<Behavior>();
        private HashSet<string> enabledInputs = new HashSet<string>();
        private HashSet<string> rightInputs = new HashSet<string>();
        private HashSet<string> leftInputs = new HashSet<string>();
        private HashSet<string[]> combos = new HashSet<string[]>();

        private Random r = new Random();
        public bool TwoHandMode = false;
        public bool SendWithDirectX = true;

        public const string START   = "START";
        public const string SELECT  = "SELECT";
        public const string UP      = "⇧";
        public const string DOWN    = "⇩";
        public const string RIGHT   = "⇨";
        public const string LEFT    = "⇦";
        public const string NONE    = "NONE";

        protected InputBoard()
        {
            keys.Add(START, VirtualKeyCode.RETURN);
            keys.Add(SELECT, VirtualKeyCode.VK_Z);
            keys.Add(UP, VirtualKeyCode.UP);
            keys.Add(DOWN, VirtualKeyCode.DOWN);
            keys.Add(RIGHT, VirtualKeyCode.RIGHT);
            keys.Add(LEFT, VirtualKeyCode.LEFT);
            keys.Add(NONE, VirtualKeyCode.NONAME);

            setLeft(UP);
            setLeft(DOWN);
            setLeft(RIGHT);
            setLeft(LEFT);
        }

        public void Enable(string value) {
            enabledInputs.Add(value);
        }

        public void Disable(string value) {
            enabledInputs.Remove(value);
        }

        public List<string> EveryInput() {
           return new List<string>(keys.Keys);
        }

        public void EnableEverything() {
            enabledInputs.UnionWith(new HashSet<string>(keys.Keys));
        }

        protected void setRight(string value)
        {
            rightInputs.Add(value);
        }

        protected void setLeft(string value)
        {
            leftInputs.Add(value);
        }

        protected void setAnyOtherKeyAsJolly()
        {
            foreach(string key in EveryInput())
            {
                if(!isLeft(key) && !isRight(key))
                {
                    setRight(key);
                    setLeft(key);
                }
            }
        }

        protected void setAnyKeyAsCombo()
        {
            foreach (string key in EveryInput())
            {
                AddCombo(key);
            }
        }

        protected void AddCombo(params string[] keys)
        {
            combos.Add(keys);
        }

        protected bool isRight(string value)
        {
            return rightInputs.Contains(value);
        }

        protected bool isLeft(string value)
        {
            return leftInputs.Contains(value);
        }


        public void PressStart()
        {
            Press(START, 150, 150);
        }

        public abstract void PressConfirm();
        

        public void Press(string key, int minHoldTime, int maxHoldTime)
        {

            PressCombo(new string[] { key }, minHoldTime, maxHoldTime);

        }

        public void PressCombo(string[] combo, int minHoldTime, int maxHoldTime)
        {

            List<VirtualKeyCode> vkcodes = new List<VirtualKeyCode>();
            foreach (string key in combo) {
                if (key != NONE && keys.Keys.Contains(key)) {
                    vkcodes.Add(keys[key]);
                }
            }
            SendInputWrapper.PressCombo(vkcodes.ToArray(), minHoldTime, maxHoldTime, SendWithDirectX, r);



        }

        public void ClearBehaviors()
        {
            behaviors.Clear();
        }

        public void AddBehavior(Behavior behavior)
        {
            behavior.SetInputBoard(this);
            behaviors.Add(behavior);
        }

        public bool ExecAdditionalBehaviors()
        {
            foreach(Behavior behavior in behaviors) {
                if (behavior.behave()) {
                    // It means this behavior did something!
                    return true;
                }
            }

            // It means no behavior did anything!
            return false;
        }

        public string NextInput(Hand hand)
        {
            string nextInput;
            do
            {
                object syncLock = new object();
                int i;
                lock (syncLock)
                {
                    i = r.Next(0, keys.Keys.Count);
                }
                nextInput = keys.Keys.ElementAt(i);
            } while (!IsValid(nextInput, hand));
            return nextInput;
        }

        public string[] NextCombo(Hand hand)
        {
            string[] nextInput;
            do
            {
                object syncLock = new object();
                int i;
                lock (syncLock)
                {
                    i = r.Next(0, combos.Count);
                }
                nextInput = combos.ElementAt(i);
            } while (!IsValid(nextInput, hand));
            return nextInput;
        }

        public string InputName(string nextInput)
        {
            return nextInput == NONE ? "" : nextInput;
        }

        public string ComboName(string[] combo)
        {
            string r = "";
            foreach (string key in combo)
                if (key != NONE)
                    r += key;

            return r;
        }

        private bool IsValid(string input, Hand hand)
        {
            bool enabled = enabledInputs.Contains(input);
            if (!enabled) return false;
            if (!TwoHandMode) return true;

            if (hand == Hand.Left)
            {
                return leftInputs.Contains(input);
            }
            else if (hand == Hand.Right)
            {
                return rightInputs.Contains(input);
            } else 
            {
                return true;
            }

        }

        internal StartAnArcadeBehavior getBehaviorOfType(StartAnArcadeBehavior startAnArcadeBehavior)
        {
            throw new NotImplementedException();
        }

        public T getBehaviorOfType<T>() where T : Behavior
        {
            foreach (Behavior b in behaviors) {
                if (b is T) return (T)b;
            }
            
            return null;
        }

        private bool IsValid(string[] combo, Hand hand)
        {
            foreach (string key in combo)
                if (!IsValid(key, hand))
                    return false;
                    
            return true;
        }
        
    }

    class GBInputBoard : InputBoard
    {

        public const string A = "A";
        public const string B = "B";

        public GBInputBoard() : base()
        {
            keys.Add(A, VirtualKeyCode.VK_S);
            keys.Add(B, VirtualKeyCode.VK_A);

            setRight(A);
            setRight(B);

            EnableEverything();
            setAnyOtherKeyAsJolly();
            setAnyKeyAsCombo();
        }

        public override void PressConfirm()
        {
            Press(A, 150, 150);
        }
    }

    class PSXInputBoard : InputBoard
    {

        public const string CROSS = "X";
        public const string CIRCLE = "O";
        public const string SQUARE = "☐";
        public const string TRIANGLE = "△";

        public PSXInputBoard() : base()
        {
            keys.Add(CROSS, VirtualKeyCode.VK_V);
            keys.Add(CIRCLE, VirtualKeyCode.VK_X);
            keys.Add(TRIANGLE, VirtualKeyCode.VK_D);
            keys.Add(SQUARE, VirtualKeyCode.VK_S);

            setRight(CROSS);
            setRight(CIRCLE);
            setRight(TRIANGLE);
            setRight(SQUARE);

            EnableEverything();
            setAnyOtherKeyAsJolly();
            setAnyKeyAsCombo();


            // Set combos
            // LEFT
            AddCombo(UP, RIGHT);
            AddCombo(DOWN, RIGHT);
            AddCombo(UP, LEFT);
            AddCombo(DOWN, LEFT);

            //RIGHT x2
            AddCombo(CROSS, SQUARE);
            AddCombo(CROSS, TRIANGLE);
            AddCombo(CROSS, CIRCLE);
            AddCombo(CIRCLE, SQUARE);
            AddCombo(CIRCLE, TRIANGLE);
            AddCombo(SQUARE, TRIANGLE);

            //RIGHT x3
            AddCombo(SQUARE, TRIANGLE, CROSS);
            AddCombo(SQUARE, TRIANGLE, CIRCLE);
            AddCombo(SQUARE, CIRCLE, CROSS);
            AddCombo(CIRCLE, TRIANGLE, CROSS);
        }

        public override void PressConfirm()
        {
            Press(CROSS, 150, 150);
        }

    }
    
 
}
