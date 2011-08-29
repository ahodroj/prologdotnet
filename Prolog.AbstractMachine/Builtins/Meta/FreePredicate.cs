using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{
    // free(Term)
    public class FreePredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "free";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsReference && X0.Reference() == X0)
            {
                program.Next();
            }
            else
            {
                state.Backtrack();
            }
        }

    }
}
