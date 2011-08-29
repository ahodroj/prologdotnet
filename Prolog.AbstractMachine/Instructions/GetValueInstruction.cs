using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class GetValueInstruction : AbstractInstruction
    {
        private string _vn;
        private string _ai;

        public override string Name()
        {
            return "get_value";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm Vn = (AbstractTerm)state[_vn];
            // Fix for Ticket #12, #4, had to dereference
            AbstractTerm Ai = ((AbstractTerm)state[_ai]).Dereference(); 

            if (!Vn.Unify(Ai))
            {
                state.Backtrack();
            }
            else
            {
                program.Next();
            }
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
