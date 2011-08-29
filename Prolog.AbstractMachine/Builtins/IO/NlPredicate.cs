using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.IO
{
    public class NlPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 0;
        }

        public override string Name()
        {
            return "nl";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            Console.WriteLine("");

            program.Next();
        }
    }
}
