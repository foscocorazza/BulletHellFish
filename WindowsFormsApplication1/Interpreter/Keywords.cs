namespace BulletHellFish
{
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
}
