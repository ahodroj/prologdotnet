using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class UnifyVoidInstruction : AbstractInstruction
    {
        private int _n;

        public override string Name()
        {
            return "unify_void";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            if (state.IsReadMode)
            {
                for (int i = 0; i < _n; i++)
                {
                    state.S = state.S.Next;
                }
            }
            else
            {
                for (int j = 0; j < _n; j++)
                {
                    heap.Push(new AbstractTerm());
                }
            }

            program.Next();

        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _n = Int32.Parse((string)arguments[0]);
        }


        public override string ToString()
        {
            return Name() + " " + _n;
        }
    }
}
