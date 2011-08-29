using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.IO
{
    public class Get0Predicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "get0";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = (state["X0"] as AbstractTerm).Dereference();

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int intValue = Convert.ToInt32(keyInfo.KeyChar);

            ConstantTerm readChar = new ConstantTerm(intValue.ToString());

            if (X0.Unify(readChar))
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
