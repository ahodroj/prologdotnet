using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class DeallocateInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "deallocate";
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            program.CP = state.E.CP;
            state.E = state.E.CE;

            program.Next();
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
        }


        public override string ToString()
        {
            return Name();
        }
    }
}
