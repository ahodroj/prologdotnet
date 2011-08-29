using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class UnifyConstantInstruction : AbstractInstruction
    {
        private string _constant;

        public override string Name()
        {
            return "unify_constant";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;
            AMTrail trail = (AMTrail)state.Trail;
            
            if (state.IsReadMode)
            {
                AbstractTerm addr = ((AbstractTerm)state.S).Dereference();
                state.S = state.S.Next;
                if (addr.IsReference)
                {
                    addr.Assign(new ConstantTerm(_constant));
                    trail.Trail(addr);
                    

                }
                else if (addr.IsConstant)
                {
                    state.Fail = !_constant.Equals(addr.Data());
                }
                else
                {
                    state.Fail = true;
                }
            }
            else
            {
                heap.Push(new ConstantTerm(_constant));
            }

            if (state.Fail)
            {
                state.Backtrack();
            }
            else
            {
                program.Next();
            }
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _constant = (string)arguments[0];
        }


        public override string ToString()
        {
            return Name() + " " + _constant;
        }
    }
}
