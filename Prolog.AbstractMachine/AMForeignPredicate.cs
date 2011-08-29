using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
    public class AMForeignPredicate
    {
        public const int R_BOOL = 0;
        public const int R_VOID = 1;
        public const int R_INTERNAL = 2;

        public AMForeignPredicate(string predicateName, string methodName, string classType, string assemblyName, int returnType)
        {
            _predicateName = predicateName;
            _methodName = methodName;
            _classType = classType;
            _assemblyName = assemblyName;
            _returnType = returnType;
        }

        private int _returnType;
        public int ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        private string _assemblyName;
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        private string _classType;
        public string ClassType
        {
            get { return _classType; }
            set { _classType = value; }
        }

        private string _methodName;
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        private string _predicateName;
        public string PredicateName
        {
            get { return _predicateName; }
            set { _predicateName = value; }
        }

        private ArrayList _arguments;
        public ArrayList Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }
        

    }
}
