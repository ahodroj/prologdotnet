using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class ProcedureInstruction : AbstractInstruction
    {
        private string _procedureName;
        public string ProcedureName
        {
            get { return _procedureName; }
            set { _procedureName = value; }
        }

        private int _arity;
        public int Arity
        {
            get { return _arity; }
            set { _arity = value; }
        }

        public override string Name()
        {
            return "procedure";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            ((AMProgram)state.Program).Next();
        }

        public override void Process(object[] arguments)
        {
            _procedureName = (string)arguments[0];
            _arity = Int32.Parse((string)arguments[1]);
        }

        public override string ToString()
        {
            return _procedureName + "/" + _arity + ":";
        }
    }
}
