using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.IO
{
    public class WritePredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "write";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            Console.Write(X0.ToString());

            program.Next();
        }
    }
}
