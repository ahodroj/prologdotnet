using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{
    // char(Term)
    public class CharPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "char";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsConstant)
            {
                string constantData = (string)X0.Data();
                constantData = constantData.Trim(new char[] { '\'' });
                if (constantData.Length == 1)
                {
                    program.Next();
                }
                else
                {
                    state.Backtrack();
                    return;
                }
            }
            else if (X0.IsReference)
            {
                if (X0.Reference() == X0)
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
