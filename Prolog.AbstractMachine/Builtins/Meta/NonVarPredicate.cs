using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Meta
{
    // nonvar(Term)
    public class NonVarPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "nonvar";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsConstant || X0.IsStructure || X0.IsList)
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
