using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Equality
{
    public class UnifyPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "=";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            
            if (X0.Unify(X1))
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
