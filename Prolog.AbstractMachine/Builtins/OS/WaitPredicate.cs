using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.OS
{
    // wait(Integer)
    public class WaitPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "wait";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            if (X0.IsReference)
            {
                throw new Exception("wait/1: argument cannot be an unbound variable.");
            }
            else if (X0.IsConstant)
            {
                int msec;
                if (!Int32.TryParse((string)X0.Data(), out msec))
                {
                    throw new Exception("wait/1: invalid wait time.");
                }
                System.Threading.Thread.Sleep(msec);
            }

            program.Next();
        }

    }
}
