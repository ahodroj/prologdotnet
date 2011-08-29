using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Comparison.Numeric
{
    public class EqualsPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "=:=";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            AbstractTerm X1 = ((AbstractTerm)state["X1"]).Dereference();

            if (TermEvaluator.Evaluate(X0) == TermEvaluator.Evaluate(X1))
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
