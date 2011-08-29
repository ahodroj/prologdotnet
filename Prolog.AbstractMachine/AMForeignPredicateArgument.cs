using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class AMForeignPredicateArgument
    {
        public const int T_INTEGER = 2;
        public const int T_DOUBLE = 99;
        public const int T_FLOAT = 3;
        public const int T_STRING = 0;
        public const int T_CHAR = 1;
        public const int T_BOOL = 5;
        public const int T_TERM = 4;

        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public const int PASS_IN = 0;
        public const int PASS_OUT = 1;
        public const int PASS_INOUT = 2;

        private int _passingType;
        public int PassingType
        {
            get { return _passingType; }
            set { _passingType = value; }
        }

        public AMForeignPredicateArgument(int type, int passingType)
        {
            _type = type;
            _passingType = passingType;
        }
    }
}
