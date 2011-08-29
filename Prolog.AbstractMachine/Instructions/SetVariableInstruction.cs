using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class SetVariableInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "set_variable";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            heap.Push(new AbstractTerm());

            AbstractTerm Vn = (AbstractTerm)state[_vn];

            Vn.Assign((AbstractTerm)heap.Top());

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
