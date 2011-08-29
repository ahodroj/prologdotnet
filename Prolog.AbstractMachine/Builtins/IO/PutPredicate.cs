using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Builtins.IO
{

    // put(Integer)
    public class PutPredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "put";
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            // print integer to output stream
            string data = (string)((AbstractTerm)state["X0"]).Dereference().Data();
            int asciiCode;

            if (!Int32.TryParse(data, out asciiCode))
            {
                throw new Exception("put/1: argument is not an integer");
            }

            Console.Write((char)asciiCode);

            program.Next();

        }
    }
}
