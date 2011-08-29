using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{

    // integer(Term)
    public class IntegerPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "integer";
        }

        public override void Execute(AbstractMachineState state)
        {
             AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsConstant)
            {
                string constantData = (string)X0.Data();
                int v;
            
                if (Int32.TryParse(constantData, out v))
                {
                    program.Next();
                }
                else
                {
                    state.Backtrack();
                    return;
                }

            }
            else
            {
                state.Backtrack();
            }
        }
    }
}
