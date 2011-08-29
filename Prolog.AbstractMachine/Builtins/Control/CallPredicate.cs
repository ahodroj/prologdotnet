using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.Control
{
    // call(Goal)
    public class CallPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "call";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm goal = ((AbstractTerm)state["X0"]).Dereference();

            if (goal.IsReference || goal.IsList)
            {
                throw new Exception("call/1: cannot call list or unbound term");
            }

            if (goal.IsConstant)
            {
                program.P = program[(string)goal.Data() + "/0"];
            }
            else if (goal.IsStructure)
            {
                for (int i = 0; i < goal.Arity; i++)
                {
                    AbstractTerm Xn = (AbstractTerm)state["X" + i.ToString()];
                    Xn.Assign(goal[i]);
                }
                program.P = program[goal.Name + "/" + goal.Arity];
            }

        }

    }
}
