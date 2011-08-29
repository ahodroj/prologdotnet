using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class SetLocalValueInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "set_local_value";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;
            
            AbstractTerm Vn = ((AbstractTerm)state[_vn]).Dereference();

            if (IsEnvironmentVariable(Vn,state))
            {
                // Push a new variable on the heap
                AbstractTerm newVariable = new AbstractTerm();
                heap.Push(newVariable);
                Vn.Bind((AbstractTerm)heap.Top());
            }
            else
            {
                AbstractTerm newHeapItem = new AbstractTerm();
                newHeapItem.Assign(Vn);
                heap.Push(newHeapItem);
            }

            program.Next();
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
