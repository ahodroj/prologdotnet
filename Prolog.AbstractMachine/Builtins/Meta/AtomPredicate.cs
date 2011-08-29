using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{
    // atom(Term).
    public class AtomPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "atom";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsReference || X0.IsList || X0.IsStructure)
            {
                state.Backtrack();
                return;
            }

            else if (X0.IsConstant)
            {
                string constantData = (string)X0.Data();

                double v;
                int u;

                if (Int32.TryParse(constantData, out u) || double.TryParse(constantData, out v))
                {
                    state.Backtrack();
                    return;
                }
            }

            program.Next();
        }

    }
}
