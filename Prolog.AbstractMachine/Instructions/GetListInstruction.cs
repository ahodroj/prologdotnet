using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class GetListInstruction : AbstractInstruction
    {
        private string _ai;

        public override string Name()
        {
            return "get_list";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Ai = ((AbstractTerm)state[_ai]).Dereference();

            if (Ai.IsReference)
            {
                heap.Push(new ListTerm());
                Ai.Bind((AbstractTerm)heap.Top());
                state.IsWriteMode = true;
            }
            else if (Ai.IsList)
            {
                state.S = Ai.Next;
                state.IsReadMode = true;
            }
            else
            {
                state.Fail = true;
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
            _ai = (string)arguments[0];
        }


        public override string ToString()
        {
            return Name() + " " + _ai;
        }
    }
}
