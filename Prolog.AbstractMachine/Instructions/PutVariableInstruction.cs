using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class PutVariableInstruction : AbstractInstruction
    {
        private string _vn;
        private string _ai;

        public override string Name()
        {
            return "put_variable";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMHeap heap = (AMHeap)state.DataArea;
            AMProgram program = (AMProgram)state.Program;


            if (_vn[0] == 'Y')
            {
                if (state.E[_vn] == null)
                {
                    throw new Exception("INTERNAL ERROR: Yn variable is null in E");
                }
                AbstractTerm Ai = (AbstractTerm)state[_ai];
                AbstractTerm Vn = (AbstractTerm)state.E[_vn];
                Ai.Assign(Vn);

                program.Next();
            }
            else
            {
                heap.Push(new AbstractTerm());

                AbstractTerm x = (AbstractTerm)state[_vn];
                AbstractTerm a = (AbstractTerm)state[_ai];

                x.Assign((AbstractTerm)heap.Top());
                a.Assign((AbstractTerm)heap.Top());

                program.Next();
            }
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _vn = (string)arguments[0];
            _ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + _vn + "," + _ai;
        }
    }
}
