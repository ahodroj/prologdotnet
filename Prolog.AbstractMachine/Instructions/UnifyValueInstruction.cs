using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class UnifyValueInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "unify_value";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Vn = ((AbstractTerm)state[_vn]).Dereference();

            if (state.IsReadMode)
            {
                state.Fail = !Vn.Unify((AbstractTerm)state.S);
            }
            else
            {
                AbstractTerm newVariable = new AbstractTerm();
                newVariable.Assign(Vn);
                heap.Push(newVariable);
            }

            state.S = state.S.Next;

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
            _vn = (string)arguments[0];
        }

        public override string ToString()
        {
            return Name() + " " + _vn;
        }

    }
}
