using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class HaltInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "halt";    
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            
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
