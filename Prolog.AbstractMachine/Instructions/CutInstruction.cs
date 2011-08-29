using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class CutInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "cut";
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;
            AMTrail trail = (AMTrail)state.Trail;

            state.B = state.E.B0;

            // tidy trail here?

            program.Next();

        }

        public override void Process(object[] arguments)
        {
            // do nothing
            _arguments = arguments;
        }


        public override string ToString()
        {
            return Name();
        }
    }
}
