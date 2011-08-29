using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class TrustMeInstruction : AbstractInstruction
    {
        public override string Name()
        {
            return "trust_me";
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

            state.B.UnsaveRegisters(state, state.B.Arity);

            state.E = state.B.CE;
            program.CP = state.B.CP;

            trail.Unwind(state.B.TR);

            trail.TR = state.B.TR;

            heap.H = state.B.H;

            state.B = state.B.B;

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
