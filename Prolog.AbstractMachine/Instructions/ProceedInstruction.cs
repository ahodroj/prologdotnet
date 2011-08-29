using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class ProceedInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "proceed";
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            program.P = program.CP;

        }

        public override void Process(object[] arguments)
        {
            // do nothing
        }

        public override string ToString()
        {
            return Name();
        }
    }
}
