using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{
    // expression is expression.
    public class IsPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "is";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            AbstractTerm X1 = ((AbstractTerm)state["X1"]).Dereference();

            if (X0.Unify(new ConstantTerm(TermEvaluator.Evaluate(X1).ToString())))
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
