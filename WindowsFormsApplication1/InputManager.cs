using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace BulletHellFish
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


        private HashSet<string[]> combosLeft = new HashSet<string[]>();
        private HashSet<string[]> combosRight = new HashSet<string[]>();
        
        private HashSet<string[]> GetCombos(Hand hand)
        {
            if (hand == Hand.Left) return combosLeft;
            return combosRight;
        }


        private Random LeftRandom = new Random();
        private Random RightRandom = new Random();
        public bool TwoHandMode = false;
        private bool SendWithDirectX = false;

        public const string START   = "START";
        public const string SELECT  = "SELECT";
        public const string UP      = "⇧";
        public const string DOWN    = "⇩";
        public const string RIGHT   = "⇨";
        public const string LEFT    = "⇦";

        internal void LiftAll()
        {
           foreach(KeyValuePair<string, VirtualKeyCode> key in keys)
            {
                SendInputWrapper.Lift(key.Value, SendWithDirectX);
            }
        }

        public const string NONE    = "NONE";

        protected InputBoard(string FilePath)
        {

            LoadFrom(FilePath);

        }

        public void Enable(string value) {
            enabledInputs.Add(value);

            foreach (string[] combo in combos)
            {
                if (combo.Contains(value))
                {
                    GetCombos(GetHand(combo)).Add(combo);
                }
            }
        }

        public void Disable(string value) {
            enabledInputs.Remove(value);
            
            combosLeft  = new HashSet<string[]>(combosLeft.Where(val => !val.Contains(value)));
            combosRight = new HashSet<string[]>(combosRight.Where(val => !val.Contains(value)));

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
            
            if(GetHand(keys) == Hand.Left)
            {
                combosLeft.Add(keys);
            } else
            {
                combosRight.Add(keys);
            }
        }

        private Hand GetHand(string[] keys)
        {
            int lefts = 0;
            int rights = 0;

            foreach (string key in keys)
            {
                if (leftInputs.Contains(key))
                    lefts++;
                else
                    rights++;
            }

            return lefts > rights ? Hand.Left : Hand.Right;
        }

        protected bool isRight(string value)
        {
            return rightInputs.Contains(value);
        }

        protected bool isLeft(string value)
        {
            return leftInputs.Contains(value);
        }

/*
        public void PressStart()
        {
            Press(START, 150, 150, Hand.Left);
        }

        public abstract void PressConfirm();*/
        

        public void Press(string key, int minHoldTime, int maxHoldTime, Hand hand)
        {

            PressCombo(new string[] { key }, minHoldTime, maxHoldTime, hand);

        }

        public void PressCombo(string[] combo, int minHoldTime, int maxHoldTime, Hand hand)
        {

            List<VirtualKeyCode> vkcodes = new List<VirtualKeyCode>();
            foreach (string key in combo) {
                if (key != NONE && keys.Keys.Contains(key)) {
                    vkcodes.Add(keys[key]);
                }
            }
            SendInputWrapper.PressCombo(vkcodes.ToArray(), minHoldTime, maxHoldTime, SendWithDirectX, GetRandom(hand));



        }

        private Random GetRandom(Hand hand)
        {
            return hand == Hand.Left ? LeftRandom : RightRandom;
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
                int i = GetRandom(hand).Next(0, keys.Keys.Count);
                nextInput = keys.Keys.ElementAt(i);
            } while (!IsValid(nextInput, hand));
            return nextInput;
        }

        public string[] NextCombo(Hand hand)
        {
            string[] nextInput;
            do
            {
                int i = GetRandom(hand).Next(0, GetCombos(hand).Count);
                nextInput = GetCombos(hand).ElementAt(i);
            } while (!IsValid(nextInput, hand));
            return nextInput;
        }

        public string[] NextComboFromNormalizedData(Hand hand, double normData)
        {
            int fourDecimals = ((int)(normData * 10000)) % 10000; // 0000-9999
            int twoDecimals = hand == Hand.Left ? (fourDecimals % 100) : (fourDecimals / 100); // 00-99
            int newIndex = (int)(((float)(GetCombos(hand).Count - 1) / (float)99) * twoDecimals);
            return GetCombos(hand).ElementAt(newIndex);

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

        public T getBehavior<T>() where T : Behavior
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

        private void LoadFrom(string FilePath)
        {
            string Mapping = "MappingMode";
            string Combos = "ComboMode";
            string Mode = Mapping;
            
                    foreach (string raw in File.ReadAllLines(FilePath))
                    {
                        string field = raw.Trim().ToUpper();
                        
                        if (field.StartsWith(",")) continue;
                        if (field.StartsWith(";")) continue;
                        if (field.StartsWith("//")) continue;

                        if (field.StartsWith(">")) {
                            if (field.Contains("MAPPING"))
                                Mode = Mapping;
                            if (field.Contains("COMBO"))
                                Mode = Combos;

                            continue;
                        }


                        if (Mode == Mapping) {
                            string[] splits = field.Split(',');
                            if (splits.Length >= 4) {
                                string strVKey = splits[1].ToUpper().Trim();
                                string strHand = splits[2].ToUpper().Trim();
                                string display = splits[3].ToUpper().Trim();

                                keys.Add(display, SendInputWrapper.GetVKeyCode(strVKey));

                                switch(strHand)
                                {
                                    case "LEFT":
                                        setLeft(display);
                                        break;
                                    case "RIGHT":
                                        setRight(display);
                                        break;
                                    default:
                                        setLeft(display);
                                        setRight(display);
                                        break;
                                }
                            }
                        }

                        if (Mode == Combos)
                        {
                            string[] splits = field.Split(',');
                            splits = splits.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            if (splits.Length > 0)
                            {
                                AddCombo(splits);
                            }
                        }



                    }

            EnableEverything();
            setAnyOtherKeyAsJolly();
            setAnyKeyAsCombo();

        }

    }


    class PSXInputBoard : InputBoard
    {
        
        public PSXInputBoard(string FilePath) : base(FilePath) {}

        /*
        public override void PressConfirm()
        {
            Press("X", 150, 150, Hand.Right);
        }*/

    }
    
 
}
