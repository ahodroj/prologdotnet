using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class SetValueInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "set_value";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Vn = new AbstractTerm();

            AbstractTerm reg = (AbstractTerm)state[_vn];

            Vn.Assign(reg.Dereference());

            heap.Push(Vn);

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
