using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class GetStructureInstruction : AbstractInstruction
    {
        private string _structureName;
        private int _arity;
        private string _ai;

        public override string Name()
        {
            return "get_structure";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            AbstractTerm Ai = ((AbstractTerm)state[_ai]).Dereference();

            if (Ai.IsReference)
            {
                heap.Push(new StructureTerm(_structureName, _arity));
                Ai.Bind((AbstractTerm)heap.Top());
                state.IsWriteMode = true;
            }
            else if (Ai.IsStructure)
            {
                if (Ai.Name == _structureName)
                {
                    // set S to next item on heap
                    state.S = Ai.Next;

                    // set read mode
                    state.IsReadMode = true;
                }
                else
                {
                    state.Fail = true;
                }
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
            char[] sep = { '/' };
            string arg0 = (string)arguments[0];

            _structureName = arg0.Split(sep, 2)[0];
            _arity = Int32.Parse(arg0.Split(sep, 2)[1]);

            _ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + _structureName + "/" + _arity + "," + _ai;
        }
    }
}
