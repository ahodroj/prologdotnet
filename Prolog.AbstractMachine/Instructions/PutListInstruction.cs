using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    /// <summary>
    /// Set register Ai to contain a LIS cell
    /// pointing to the current top of the heap.
    /// Continue execution with the following instruction.
    /// </summary>
    public class PutListInstruction : AbstractInstruction
    {
        private string _ai;

        public override string Name()
        {
            return "put_list";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Ai = (AbstractTerm)state[_ai];

            heap.Push(new ListTerm());

            Ai.Assign((AbstractTerm)heap.Top());

            program.Next();
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
