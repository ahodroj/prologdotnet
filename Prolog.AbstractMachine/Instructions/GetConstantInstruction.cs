using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class GetConstantInstruction : AbstractInstruction
    {
        private string _constant;
        private string _ai;

        public override string Name()
        {
            return "get_constant";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm Ai = ((AbstractTerm)state[_ai]).Dereference();

            if (Ai.IsReference)
            {
                Ai.Assign(new ConstantTerm(_constant));
                AMTrail trail = (AMTrail)state.Trail;
                trail.Trail(Ai);
            }
            else if (Ai.IsConstant)
            {
                state.Fail = ((string)Ai.Data() != _constant);
            }
            else
            {
                state.Fail = true;
            }

            if (state.Fail)
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
            _constant = (string)arguments[0];
            _ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + _constant + "," + _ai;
        }
    }
}
