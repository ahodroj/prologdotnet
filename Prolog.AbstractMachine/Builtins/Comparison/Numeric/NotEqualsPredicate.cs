using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Comparison.Numeric
{
    public class NotEqualsPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "=\\=";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            int op1;
            int op2;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            AbstractTerm X1 = ((AbstractTerm)state["X1"]).Dereference();

            if (!Int32.TryParse((string)X0.Data(), out op1) || !Int32.TryParse((string)X1.Data(), out op2))
            {
                throw new Exception("==/2: Cannot compare two non-numeric terms.");
            }

            op1 = Int32.Parse((string)X0.Data());
            op2 = Int32.Parse((string)X1.Data());

            if (op1 != op2)
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
