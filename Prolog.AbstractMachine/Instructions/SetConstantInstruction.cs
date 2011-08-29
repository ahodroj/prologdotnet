using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class SetConstantInstruction : AbstractInstruction
    {
        private string _constant;

        public override string Name()
        {
            return "set_constant";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            heap.Push(new ConstantTerm(_constant));

            program.Next();
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
