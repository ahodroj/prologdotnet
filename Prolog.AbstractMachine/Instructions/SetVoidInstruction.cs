using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    /// <summary>
    /// In read mode, place the contents of
    /// heap address S into variable Vn; in
    /// write mode, push a new unbound
    /// REF cell onto the heap and copy it
    /// into Xi. In either mode, increment S
    /// by one. Continue execution with the
    /// following instruction.
    /// </summary>
    public class SetVoidInstruction : AbstractInstruction
    {
        private int _n;

        public override string Name()
        {
            return "set_void";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMHeap heap = (AMHeap)state.DataArea;
            AMProgram program = (AMProgram)state.Program;

            for (int i = 0; i < _n; i++)
            {
                heap.Push(new AbstractTerm());
            }

            program.Next();
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _n = Int32.Parse((string)arguments[0]);
        }


        public override string ToString()
        {
            return Name() + " " + _n;
        }
    }
}
