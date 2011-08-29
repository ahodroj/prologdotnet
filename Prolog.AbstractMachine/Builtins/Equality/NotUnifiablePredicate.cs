using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Equality
{
    // Term \= Term
    public class NotUnifiablePredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "\\=";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            AbstractTerm term1 = new AbstractTerm();
            AbstractTerm term2 = new AbstractTerm();
            term1.Assign(X0);
            term2.Assign(X1);

            if (!term1.Unify(term2))
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
