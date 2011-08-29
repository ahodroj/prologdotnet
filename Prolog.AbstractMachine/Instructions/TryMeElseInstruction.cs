using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class TryMeElseInstruction : AbstractInstruction
    {
        private string _label;

        public override string Name()
        {
            return "try_me_else";
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

            ProgramClause nextClause = program[_label];
            Choicepoint B = new Choicepoint(program.NumberOfArguments, state.E, program.CP, state.B, nextClause, trail.TR, heap.H);
            B.SaveRegisters(state, program.NumberOfArguments);
            state.B = B;

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
