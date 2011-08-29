using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class UnifyVariableInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "unify_variable";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Vn = (AbstractTerm)state[_vn];
            if (state.IsReadMode)
            {
                Vn.Assign((AbstractTerm)state.S);
            }
            else
            {
                heap.Push(new AbstractTerm());
                Vn.Assign((AbstractTerm)heap.Top());
            }
            if (state.S != null)
            {
                state.S = state.S.Next;
            }
            program.Next();
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
