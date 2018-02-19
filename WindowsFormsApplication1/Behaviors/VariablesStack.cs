using System.Collections.Generic;

namespace BulletHellFish
{
    class VariablesStack
    {
        Dictionary<string, int> ints = new Dictionary<string, int>();
        Dictionary<string, string> strings = new Dictionary<string, string>();
        Dictionary<string, bool> bools = new Dictionary<string, bool>();

        public int GetInt(string name)
        {
            int var;
            if (ints.TryGetValue(name, out var))
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

        public bool Exists(string name)
        {
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
}
