using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    /// <summary>
    /// Place a constant cell containing c into 
    /// register Ai. Continue execution with the 
    /// following instruction.
    /// </summary>
    public class PutConstantInstruction : AbstractInstruction
    {
        private string _c;
        private string _ai;

        public override string Name()
        {
            return "put_constant";
        }

        public override int NumberOfArguments()
        {
            return 2;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm Ai = (AbstractTerm)state[_ai];

            Ai.Assign(new ConstantTerm(_c));

            program.Next();
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _c = (string)arguments[0];
            _ai = (string)arguments[1];
        }


        public override string ToString()
        {
            return Name() + " " + _c + "," + _ai;
        }
    }
}
