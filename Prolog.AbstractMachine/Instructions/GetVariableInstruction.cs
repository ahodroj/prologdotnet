using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class GetVariableInstruction : AbstractInstruction
    {
        private string _vn;
        private string _ai;

        public override string Name()
        {
            return "get_variable";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm Vn = (AbstractTerm)state[_vn];
            AbstractTerm Ai = (AbstractTerm)state[_ai];

            Vn.Assign(Ai);

            program.Next();

        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _vn = (string)arguments[0];
            _ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + _vn + "," + _ai;
        }
    }
}
