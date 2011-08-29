using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class UnifyLocalValueInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "unify_local_value";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Vn = (AbstractTerm)state[_vn];

            if (state.IsReadMode)
            {
                state.Fail = !Vn.Unify((AbstractTerm)state.S);
            }
            else
            {
                AbstractTerm addr = Vn.Dereference();

                if (IsEnvironmentVariable(addr, state))
                {
                    // Push a new variable on the heap
                    AbstractTerm newVariable = new AbstractTerm();
                    heap.Push(newVariable);
                    addr.Bind((AbstractTerm)heap.Top());
                }
                else
                {
                    AbstractTerm newHeapItem = new AbstractTerm();
                    newHeapItem.Assign(addr);
                    heap.Push(newHeapItem);
                }

            }
            if (state.S != null)
            {
                state.S = state.S.Next;
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
            _vn = (string)arguments[0];
        }

        private bool IsEnvironmentVariable(AbstractTerm Vn, AbstractMachineState state)
        {
            // required, otherwise it will crash
            if (state.E == null)
                return false;

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
            return Name() + " " + _vn;
        }
    }
}
