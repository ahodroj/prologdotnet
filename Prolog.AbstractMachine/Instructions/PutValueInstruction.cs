using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class PutValueInstruction : AbstractInstruction
    {
        private string vn;
        private string ai;

        public override string Name()
        {
            return "put_value";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMHeap heap = (AMHeap)state.DataArea;
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm Vn = (AbstractTerm)state[vn];
            AbstractTerm Ai = (AbstractTerm)state[ai];
             
            Ai.Assign(Vn);
            program.Next();
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            vn = (string)arguments[0];
            ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + vn + "," + ai;
        }
    }
}
