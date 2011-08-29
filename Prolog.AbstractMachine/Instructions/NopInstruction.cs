using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class NopInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "nop";    
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            program.Next();
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
