using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    /// <summary>
    /// Push a new functor cell containing a structure 
    /// onto the heap and set register Ai to that functor cell.
    /// Continue execution with the following instruction.
    /// </summary>
    public class PutStructureInstruction : AbstractInstruction
    {
        private string structureName;
        private int arity;
        private string ai;

        public override string Name()
        {
            return "put_structure";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMHeap heap = (AMHeap)state.DataArea;
            AMProgram program = (AMProgram)state.Program;

            heap.Push(new StructureTerm(structureName, arity));

            AbstractTerm Ai = (AbstractTerm)state[ai];

            Ai.Assign((AbstractTerm)heap.Top());

            program.Next();
        }

        public override void Process(object[] arguments)
        {
            
            _arguments = arguments;
            char[] sep = { '/' };
            string arg0 = (string)arguments[0];
            // Bug fix
            if (arg0 == "//2")
            {
                structureName = "/";
                arity = 2;
            }
            else
            {
                structureName = arg0.Split(sep, 2)[0];
                arity = Int32.Parse(arg0.Split(sep, 2)[1]);
            }
            ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + structureName + "/" + arity + "," + ai;
        }
    }
}
