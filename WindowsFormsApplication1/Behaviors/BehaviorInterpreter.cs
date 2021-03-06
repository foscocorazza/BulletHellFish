﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Result = System.Collections.Generic.KeyValuePair<int, object>;

namespace BulletHellFish
{
    class BehaviorInterpreter
    {

        const string QUOTE = "\"";
   
        VariablesStack vars = new VariablesStack();
        Behavior behavior;
        string[] program;
        bool behaved = false;

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
            IsSimilarTo, LogicOperation
        }

        int programCounter = 0;

        public bool Execute() {
            programCounter = 0;
            return Execute(program, 0);
        }

        private bool Execute(string[] program, int currentLine)
        {
            for (int i = 0; i < program.Length;)
            {
                string line = program[i];
                programCounter = currentLine + i;

                i += Eval(line).Key;

            }

            return behaved;

        }
        
        // Key: number of line executed
        // Value: operation outcome
        private Result Eval(string line)
        {
            string[] content = GetContentInside(line);
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
                    RepeatLoop(line, content, programCounter);
                    lineExecuted = content.Length+1;
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
                case LineType.If:
                    If(line, content, programCounter);
                    lineExecuted = content.Length+1;
                    break;
                case LineType.IsSimilarTo:
                    result = IsSimilarTo(line);
                    break;
                case LineType.MathOperation:
                    result = MathOperation(line);
                    break;
                case LineType.LogicOperation:
                    result = LogicOperation(line);
                    break;
            }
            return new Result(lineExecuted, result);
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

            if (ContainsOneOf(line, Keywords.LogicOperation))
            {
                return LineType.LogicOperation;
            }

            if (ContainsOneOf(line, Keywords.MathOperation))
            {
                return LineType.MathOperation;
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
                value = Merge(components, 3);
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
            if(components.Length > 2)
            {
                SetVar("var", components[0], Merge(components, 2));
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

        private void If(string line, string[] content, int currentLine)
        {
            string[] components = RemoveFirst(TrimSplit(line)); // Ignoring keyword
            bool value = EvaluateAsBool(components);

            if(value)
            {
                Execute(content, currentLine + 1);
            }

        }

        private void SetVar(string type, string name, string value)
        {

            // First: check for literals
            if (TrimSplit(value).Length == 1)
            {

                // Is Bool or Inferred as bool: 
                bool isBool = StartsWithOneOf(type, Keywords.Bool); // Declared as bool
                isBool = isBool || (StartsWithOneOf(type, Keywords.InferVar) && IsOneOf(value, Keywords.BoolValue)); // declared as Var, but inferred as bool
                if (isBool)
                {
                    vars.SetBool(name, StringToBool(value));
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

                // Is a StringVariable
                if (vars.ExistsAsString(value))
                {
                    vars.SetString(name, vars.GetString(value));
                    return;
                }

                // Is a BoolVariable
                if (vars.ExistsAsBool(value))
                {
                    vars.SetBool(name, vars.GetBool(value));
                    return;
                }

                // Is an IntVariable
                if (vars.ExistsAsInt(value))
                {
                    vars.SetInt(name, vars.GetInt(value));
                    return;
                }

                // It is a literal int, but the variable is a string.
                if (StartsWithOneOf(type, Keywords.Str) && itsAnInt)
                {
                    vars.SetString(name, value);
                    return;
                }

            } else
            {
                // Value it's an expression;
                Result result = Eval(value);
                Type t = result.Value.GetType();

                if (t == typeof(int)) {
                    SetVar("int", name, result.Value.ToString());
                } else if (t == typeof(bool))
                {
                    SetVar("bool", name, result.Value.ToString());
                }
                else if(t == typeof(string)) {
                    SetVar("str", name, result.Value.ToString());
                }

            }

           
            Error();

        }

        private int MathOperation(string line)
        {
            // TODO;
            string[] fields = SplitByAnyOf(line, Keywords.MathOperation);
            return 1;
        }

        private bool LogicOperation(string line)
        {
            string[] fields = SplitByAnyOf(line, Keywords.LogicOperation);
            string[] fieldsWithOp = TrimSplit(line);

            if (fieldsWithOp.Length == 0) return false;
            if (fieldsWithOp.Length == 1 || fieldsWithOp.Length == 2) return EvaluateAsBool(new string[] { fields[0] });

            bool val = LogicOperationTriplette(fieldsWithOp);

            /*for (int i = 0; i < fieldsWithOp.Length - 1; i++)
            {

            }*/

            return val;
        }

        // Operate only the first 3
        private bool LogicOperationTriplette(string[] lines)
        {
            if (lines.Length < 3) return false;
            string op = lines[1];
            bool val1 = EvaluateAsBool(new string[] { lines[0] });
            bool val2 = EvaluateAsBool(new string[] { lines[2] });


            if (IsOneOf(op, Keywords.Or))
            {
                return val1 || val2;
            }

            if (IsOneOf(op, Keywords.And))
            {
                return val1 && val2;
            }


            Error();
            return false;
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

                string[] first = TrimSplit(fields[0]);
                if (first.Length == 0) {
                    Error();
                    return false;
                }
                string image = EvaluateAsString(new string[] { Trim(first[0]) });
                
                float percentage = -1;
                if (first.Length > 1)
                {
                    // Then there is percentage as well
                    string pline = Trim(first[1]).Replace("%", "");
                    percentage = EvaluateAsInt(new string[] { pline });
                    percentage = ((float)percentage) / 100.0f;
                }
                
                if (fields.Length == 1)
                {
                    // Done! Just give the image to the behavior!
                    return behavior.IsSimilarTo(image, percentage);
                }
                else if (fields.Length == 2)
                {
                    string second = Trim(fields[1]);

                    if (StartsWithOneOf(second, Keywords.InAt))
                    {

                        
                        // Then program, asks for rectangle.
                        System.Drawing.Rectangle rect = GetRect(RemoveFirst(TrimSplit(second)));
                        return behavior.IsSimilarTo(image, rect, percentage);
                    }
                    else {
                        // probably percentage, but for now:

                        return behavior.IsSimilarTo(image, percentage);

                    }

                } else
                {
                    // TODO: Three or more? it means to "similar" in the same line? nonsens!
                    Error();
                    return false;

                }

            }
            

        }


        #region Eval

        private bool EvaluateAsBool(string[] components)
        {
            if (components.Length == 0)
            {
                Error();
                return false;
            }

            if (components.Length == 1)
            {
                string val = components[0];
                if (IsOneOf(val, Keywords.BoolValue))
                {
                    return StringToBool(val);
                }
                else if (vars.ExistsAsBool(val))
                {
                    return vars.GetBool(val);
                } else
                {
                    Error();
                    return false;
                }
            }

            Result e = Eval(Merge(components, 0));
            if(e.Value.GetType() == typeof(bool))
            {
                return (bool)e.Value;
            } else
            {
                Error();
                return false;
            }
                 
        }

        private bool StringToBool(string val)
        {
            return !IsOneOf(val, Keywords.False);
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
                    return RemoveQuotes(val);
                }
            }

            // TODO: More than one compo? Merge them?
            Error();
            return "";
        }

        #endregion

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

        private string Merge(string[] components, int start, int length) {
            string result = "";
            int L = components.Length;
            if (start < L)
            {
                int realLength = length;
                if(start+length > L)
                {
                    realLength = L - start;
                }

                for(int i = start; i< realLength+start; i++)
                {
                    result += components[i] + " ";
                }
            }
            return Trim(result);
        }

        private string Merge(string[] components, int start)
        {
            return Merge(components, start, components.Length);
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
            string[] components = TrimSplit(line);
            foreach (string str in strs)
            {
                // TODO: check not in strings
                foreach (string component in components)
                {
                    if (component.Equals(str)) return true;
                }
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

        public bool IsOneOf(string line, string[] strs)
        {

            foreach (string str in strs)
            {
                if (line.ToLower().Equals(str.ToLower()))
                {
                    return true;
                }
            }

            return false;

        }


        public bool IsOneOf(string line, string strs)
        {
            return IsOneOf(line, strs.Split('|'));
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
