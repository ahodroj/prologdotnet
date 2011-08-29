using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.IO
{
    public class WriteLnPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "writeln";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            Console.WriteLine(state["X0"]);

            program.Next();
        }
    }
}
