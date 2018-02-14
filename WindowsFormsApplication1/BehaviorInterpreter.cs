using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class BehaviorInterpreter
    {


        const string QUOTE = "\"";

        class Keywords
        {
            public const string If = "if";
            public const string Repeat = "repeat";
            public const string Try = "try";
            public const string Times = "times|time";
            public const string Wait = "wait";
            public const string Press = "press";
            public const string Seconds = "s|second|seconds";
            public const string Milliseconds = "ms|millisecond|milliseconds";
            public const string Int = "int|integer";
            public const string Bool = "bool|boolean|yesno";
            public const string Str = "str|string|phrase";
            public const string Lock = "lock";
            public const string Unlock = "unlock";
            public const string EqualsSymbol = "=|is|equals|set";
            public const string InferVar = "var|auto";
            public const string IsSimilarTo = "similar";
            public const string InAt = "in|at";
            public const string False = "0|false|no";
            public const string True = "1|true|yes";

            public const string BoolValue = True + "|" + False;
            public const string Vars = Int + "|" + Bool + "|" + Str + "|" + InferVar;

        }

        class Variables
        {
            Dictionary<string, int> ints = new Dictionary<string, int>();
            Dictionary<string, string> strings = new Dictionary<string, string>();
            Dictionary<string, bool> bools = new Dictionary<string, bool>();

            public int GetInt(string name)
            {
                int var;
                if(ints.TryGetValue(name, out var))
                {
                    return var;
                }
                return 0;
            }

            public string GetString(string name)
            {
                string var;
                if (strings.TryGetValue(name, out var))
                {
                    return var;
                }
                return "";
            }

            public bool GetBool(string name)
            {
                bool var;
                if (bools.TryGetValue(name, out var))
                {
                    return var;
                }
                return false;
            }

            public void SetInt(string name, int value)
            {
                ints[name] = value;
            }
     
            public void SetBool(string name, bool value)
            {
                bools[name] = value;
            }

            public void SetString(string name, string value)
            {
                strings[name] = value;
            }

            public bool Exists(string name) {
                return ints.ContainsKey(name) || bools.ContainsKey(name) || strings.ContainsKey(name);
            }

            public bool ExistsAsString(string name)
            {
                return strings.ContainsKey(name);
            }
            public bool ExistsAsBool(string name)
            {
                return bools.ContainsKey(name);
            }
            public bool ExistsAsInt(string name)
            {
                return ints.ContainsKey(name);
            }

        }

        Variables vars = new Variables();
        Behavior behavior;
        string[] program;

        public BehaviorInterpreter(Behavior behavior, string[] program)
        {
            this.behavior = behavior;
            this.program = program;
        }

        enum LineType
        {
            RepeatLoop, TryLoopTop, TryLoopEnd,
            Press, Behave, Unbehave, Wait, If, 
            DeclareVar, SetVar, GetVar, IncreaseVar, DecreaseVar,
            MathOperation, Comment, Lock, Unlock,
            IsSimilarTo
        }

        int programCounter = 0;

        public bool Execute() {
            programCounter = 0;
            return Execute(program, 0);
        }

        private bool Execute(string[] program, int currentLine)
        {
            for (int i = 0; i < program.Length; i++)
            {
                string line = program[i];
                programCounter = currentLine + i;

                i += ExecLine(line).Key;

            }

            return true;

        }


        // Key: number of line executed
        // Value: operation outcome
        private KeyValuePair<int, object> ExecLine(string line)
        {
            int lineExecuted = 1;
            object result = null;
            switch (TypeOf(line))
            {
                case LineType.SetVar:
                    SetVar(line);
                    break;
                case LineType.DeclareVar:
                    DeclareVar(line);
                    break;
                case LineType.RepeatLoop:
                    string[] content = GetContentInside(line);
                    RepeatLoop(line, content, programCounter);
                    lineExecuted = content.Length;
                    break;
                case LineType.Wait:
                    Wait(line);
                    break;
                case LineType.Press:
                    Press(line);
                    break;
                case LineType.Lock:
                    Behavior.Lock();
                    break;
                case LineType.Unlock:
                    Behavior.Unlock();
                    break;
                case LineType.IsSimilarTo:
                    result = IsSimilarTo(line);
                    break;
            }
            return new KeyValuePair<int, object>(lineExecuted, result);
        }

        private LineType TypeOf (string rawLine)
        {
            string line = Trim(rawLine);

            if (string.IsNullOrWhiteSpace(line))
                return LineType.Comment;

            if (StartsWithOneOf(line, Keywords.If))
            {
                return LineType.If;
            }

            if (StartsWithOneOf(line, Keywords.Vars))
            {
                return LineType.DeclareVar;
            }

            if (StartsWithOneOf(line, Keywords.Wait))
            {
                return LineType.Wait;
            }

            if (StartsWithOneOf(line, Keywords.Press))
            {
                return LineType.Press;
            }

            if (StartsWithOneOf(line, Keywords.Unlock))
            {
                return LineType.Unlock;
            }

            if (StartsWithOneOf(line, Keywords.Lock))
            {
                return LineType.Lock;
            }

            if (StartsWithOneOf(line, Keywords.Repeat))
            {
                return LineType.RepeatLoop;
            }

            if (StartsWithOneOf(line, Keywords.Vars))
            {
                return LineType.DeclareVar;
            }

            if (ContainsOneOf(line, Keywords.IsSimilarTo))
            {
                return LineType.IsSimilarTo;
            }


            return LineType.SetVar;
        }


        #region Actions

        private void DeclareVar(string line)
        {
            string[] components = TrimSplit(line);
            string type = components[0];
            string name = components[1];
            string value = "";
            if (components.Length > 3 && StartsWithOneOf(components[2], Keywords.EqualsSymbol))
            {
                value = components[3];
                SetVar(type, name, value);
            }
            else if (!StartsWithOneOf(type, Keywords.InferVar))
            {
                SetVar(type, name, "0");
            }
            else {
                // Ignore it, come on, you declared a variable without type NOR value.
            }
            
            
        }

        private void SetVar(string line) {
            string[] components = TrimSplit(line);
            if(components.Length>2)
            {
                SetVar("var", components[0], components[2]);
            }

        }

        private void RepeatLoop(string line, string[] content, int currentLine)
        {
            string[] components = RemoveFirst(TrimSplit(line)); // Ignoring keyword
            if(StartsWithOneOf(Last(components), Keywords.Times))
            {
                components = RemoveLast(components);
            }

            int times = EvaluateAsInt(components);

            for(int i = 0; i < times; i++)
            {
                Execute(content, currentLine+1);
            }

        }

        private void SetVar(string type, string name, string value)
        {

            // First: check for literals
            if (TrimSplit(value).Length == 1)
            {

                // Is Bool or Inferred as bool: 
                bool isBool = StartsWithOneOf(type, Keywords.Bool); // Declared as bool
                isBool = isBool || (StartsWithOneOf(type, Keywords.InferVar) && StartsWithOneOf(value, Keywords.BoolValue)); // declared as Var, but inferred as bool
                if (isBool)
                {
                    vars.SetBool(name, !StartsWithOneOf(value, Keywords.False));
                    return;
                }

                // Is Int or Inferred as int: 
                int intvalue = 0;
                bool itsAnInt = int.TryParse(value, out intvalue);
                bool isInt = StartsWithOneOf(type, Keywords.Int); // Declared as int
                isInt = isInt || (StartsWithOneOf(type, Keywords.InferVar) && itsAnInt); // declared as Var, but inferred as int

                if (isInt)
                {
                    vars.SetInt(name, intvalue);
                    return;
                }

                // Is String or Inferred as string: 
                bool isString = StartsWithOneOf(type, Keywords.Str); // Declared as string
                isString = isString || (StartsWithOneOf(type, Keywords.InferVar) && IsLiteralString(value)); // declared as Var, but inferred as str

                if (isString)
                {
                    vars.SetString(name, RemoveQuotes(value));
                    return;
                }

            }

            // I finished inferring things. Still, could be:
            // 1. A variable;
            // 2. An expression;
            // 3. Trying to put an int in a "string" var;
            // 4. A non-valid sequence of characters;

            // 1. Is it a variable?
            if (vars.ExistsAsString(value)) {
                vars.SetString(name, vars.GetString(value));
                return;
            }

            if (vars.ExistsAsBool(value))
            {
                vars.SetBool(name, vars.GetBool(value));
                return;
            }

            if (vars.ExistsAsInt(value))
            {
                vars.SetInt(name, vars.GetInt(value));
                return;
            }

            // 2. Is it an expression?
            // TODO

            // 3. Trying put int in string
            if(StartsWithOneOf(type, Keywords.Str) && itsAnInt)
            {
                vars.SetString(name, value);
                return;
            }

            // 4. A non-valid sequence of characters;
            Error();

        }


        private void Wait(string line)
        {
            string[] components = TrimSplit(line);
            // First compo is keyword. Ignored
            // Last compo is unit of measure
            string unit = "s";
            if (StartsWithOneOf(Last(components), Keywords.Seconds))
            {
                unit = "s";
            }

            if (StartsWithOneOf(Last(components), Keywords.Milliseconds))
            {
                unit = "ms";
            }


            // Mid compos are value
            string[] valueExpression = RemoveLast(RemoveFirst(components));
            int value = EvaluateAsInt(valueExpression);
            if(unit == "s")
            {
                value *= 1000;
            }

            behavior.Wait(value);
            
        }

        private void Press(string line)
        {
            string[] components = TrimSplit(line);
            // First compo is keyword. Ignored

            // Every other is a string
            // TODO: timing
            string[] valueExpression = RemoveFirst(components);
            string value = EvaluateAsString(valueExpression);

            behavior.Press(value);

        }


        private bool IsSimilarTo(string line) {
            string[] fields = SplitByAnyOf(line, Keywords.IsSimilarTo);
            
            if (fields.Length == 0)
            {
                Error();
                return false;
            } else {
                string image = EvaluateAsString(new string[]{ Trim(fields[0]) });
                bool multicheck = image.EndsWith("*");


                if (fields.Length == 1)
                {
                    // Done! Just give the image to the behavior!
                    return behavior.IsSimilarTo(image, multicheck);
                }
                else if (fields.Length == 2) {
                    string second = Trim(fields[1]);
                    if (StartsWithOneOf(second, Keywords.InAt))
                    {
                        // Then program, asks for rectangle.
                        System.Drawing.Rectangle rect = GetRect(RemoveFirst(TrimSplit(second)));
                        return behavior.IsSimilarTo(image, multicheck, rect);
                    }
                    else {
                        // probably percentage, but for now:

                        return behavior.IsSimilarTo(image, multicheck);

                    }

                } else
                {
                    // TODO: Three or more? it means to "similar" in the same line? nonsens!
                    Error();
                    return false;

                }

            }
            

        }

        private int EvaluateAsInt(string[] components)
        {
            if(components.Length == 0)
            {
                Error();
                return 0;
            }

            if(components.Length == 1)
            {
                int i = 0;
                if( int.TryParse(components[0], out i))
                {
                    return i;
                } else
                {
                    return vars.GetInt(components[0]);
                }
            }

            // TODO: More than one compo? 
            Error();
            return 0;
        }

        private string EvaluateAsString(string[] components)
        {
            if (components.Length == 0)
            {
                Error();
                return "";
            }

            if (components.Length == 1)
            {
                string val = components[0];
                if (vars.ExistsAsString(val))
                {
                    return vars.GetString(val);
                }
                else
                {
                    return val;
                }
            }

            // TODO: More than one compo? Merge them?
            Error();
            return "";
        }

        private void Error()
        {

        }

        #endregion

        #region Utils


        public string[] GetContentInside(string line)
        {
            int level = GetLineLevel(line);
            List<string> content = new List<string>();
            
            for(int i = programCounter+1; i< program.Length; i++)
            {
                if(GetLineLevel(program[i])>level)
                {
                    content.Add(program[i]);
                } else
                {
                    break;
                }
            }

            return content.ToArray();
        }

        private int GetLineLevel(string line)
        {
            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line.ToCharArray()[i] == '\t') {
                    level++;
                } else if (line.Substring(i, 4).Equals("    ")) {
                    i += 3;
                    level++;
                } else
                {
                    break;
                }
            }

            return level;
        }


        public string Trim(string line) {
            
            const string reduceMultiSpace = @"[ ]{2,}";
            return Regex.Replace(line.Replace("\t", " "), reduceMultiSpace, " ").Trim();

        }

        public string RemoveQuotes(string line)
        {

            string newline = line;

            if (newline.StartsWith(QUOTE))
            {
                newline = newline.Substring(1);
            }

            if (newline.EndsWith(QUOTE))
            {
                newline = newline.Substring(0, newline.Length - 1);
            }

            return newline;

        }

        public string[] TrimSplit(string line)
        {
            //TODO: Check for strings
            string[] components = Trim(line).Split(' ');
            string[] trimmedComponents = new string[components.Length];
            for(int i = 0; i < components.Length; i++)
            {
                trimmedComponents[i] = Trim(components[i]);
            }
            return trimmedComponents;

        }


        public bool IsLiteralString(string rawLine)
        {
            string line = Trim(rawLine);
            

            return line.Length>1 && line.StartsWith(QUOTE) && line.EndsWith(QUOTE);

        }

        public string Last(string[] components)
        {
            if(components.Length > 0) return components[components.Length - 1];
            return "";
        }

        public string[] RemoveLast(string[] components)
        {
            if (components.Length > 0) {
                return SubArray(components, 0, components.Length - 1);
            }

            return new string[0];
        }
        
        public string[] RemoveFirst(string[] components)
        {
            if (components.Length > 0)
            {
                return SubArray(components, 1, components.Length - 1);
            }

            return new string[0];
        }

        public bool StartsWithOneOf(string line, string[] strs)
        {

            foreach (string str in strs) {
                if (line.StartsWith(str)) return true;
            }

            return false;

        }

        public bool ContainsOneOf(string line, string[] strs)
        {

            foreach (string str in strs)
            {
                // TODO: check not in strings
                if (line.Contains(str)) return true;
            }

            return false;

        }

        public string[] SplitByAnyOf(string line, string[] strs)
        {

            foreach (string str in strs)
            {
                // TODO: check not in strings
                if (line.Contains(str)) {
                    return line.Split(new string[] { str }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            return new string[0];

        }


        public string[] SplitByAnyOf(string line, string strs)
        {
            return SplitByAnyOf(line, strs.Split('|'));
        }


        public bool StartsWithOneOf(string line, string strs)
        {
            return StartsWithOneOf(line, strs.Split('|'));
        }

        public bool ContainsOneOf(string line, string strs)
        {
            return ContainsOneOf(line, strs.Split('|'));
        }


        public T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }


        public System.Drawing.Rectangle GetRect(string[] values)
        {
            System.Drawing.Rectangle error = new System.Drawing.Rectangle();

            if (values.Length < 2) {
                Error();
                return error;
            } else
            {
                int x, y, w=-1, h=-1;

                if (!int.TryParse(values[0], out x) || !int.TryParse(values[1], out y)) {
                    Error();
                    return error;
                }

                if (values.Length >= 4)
                {
                    int.TryParse(values[2], out w);
                    int.TryParse(values[3], out h);
                }
                
                return new System.Drawing.Rectangle(x, y, w, h);
            }

        }
        #endregion

    }
}
