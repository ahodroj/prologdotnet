using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class PutUnsafeValueInstruction : AbstractInstruction
    {
        private string _yn;
        private string _ai;

        public override string Name()
        {
            return "put_unsafe_value";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;


            AbstractTerm Yn = state.E[_yn].Dereference();
            AbstractTerm Ai = (AbstractTerm)state[_ai];

            if (!IsEnvironmentVariable(Yn, state))
            {
                Ai.Assign(Yn);
            }
            else
            {
                heap.Push(new AbstractTerm());
                Yn.Bind((AbstractTerm)heap.Top());
                Ai.Assign(Yn);
            }

            program.Next();

        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _yn = (string)arguments[0];
            _ai = (string)arguments[1];
        }

        private bool IsEnvironmentVariable(AbstractTerm Vn, AbstractMachineState state)
        {
            for (AbstractTerm a = state.E.PermanentVariables; a != null; a = (AbstractTerm)a.Next)
            {
                if (a == Vn)
                {
                    return true;
                }
            }
            return false;
        }


        public override string ToString()
        {
            return Name() + " " + _yn + "," + _ai;
        }
    }
}
