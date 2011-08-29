using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class RetryMeElseInstruction : AbstractInstruction
    {
        private string _label;

        public override string Name()
        {
            return "retry_me_else";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMTrail trail = (AMTrail)state.Trail;
            AMHeap heap = (AMHeap)state.DataArea;

            int n = state.B.Arity;

            state.B.UnsaveRegisters(state, n);

            state.E = state.B.CE;
            program.CP = state.B.CP;
            state.B.NextClause = program[_label];

            trail.Unwind(state.B.TR);

            trail.TR = state.B.TR;

            heap.H = state.B.H;

            // TODO: what about HB ?

            program.Next();

        }

        public override void Process(object[] arguments)
        {
            _label = (string)arguments[0];
        }


        public override string ToString()
        {
            return Name() + " " + _label;
        }
    }
}
