using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class FailInstruction : AbstractInstruction
    {
        

        public override string Name()
        {
            return "fail";
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            state.Backtrack();
        }

        public override void Process(object[] arguments)
        {
           
        }


        public override string ToString()
        {
            return Name();
        }
    }
}
