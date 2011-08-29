using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class AllocateInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "allocate";
        }

        public override int NumberOfArguments()
        {
            return 0;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            EnvironmentFrame env = new EnvironmentFrame(state.E, program.CP, state.B0, 2);
            
            // Is this really needed?
            heap.Push(env);

            state.E = (EnvironmentFrame)heap.Top();

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
