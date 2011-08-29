using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.Instructions
{
    public class CallVariableInstruction : AbstractInstruction
    {
        private string _vn;

        public override string Name()
        {
            return "callvar";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;

            AbstractTerm X0 = (AbstractTerm)state[_vn];


            string predicateName = null;
            int predicateArity = 0;

            if (X0.IsList)
            {
                throw new Exception("callvar: cannot call a list.");
            }
            else if (X0.IsReference)
            {
                throw new Exception("callvar: cannot call an unbound variable.");
            }
            else if (X0.IsConstant)
            {
                int val;

                if (Int32.TryParse((string)X0.Data(), out val))
                {
                    throw new Exception("callvar: cannot call an int.");
                }
                predicateName = (string)X0.Data();
            }
            else if (X0.IsStructure)
            {
                // TODO: should we maybe handle built-in predicates?
                predicateName = X0.Name;
                predicateArity = X0.Arity;
            }

             if (program.IsDefined(predicateName + "/" + predicateArity))
            {
                program.CP = program.P.Next;
                program.NumberOfArguments = predicateArity;
                program.P = program[predicateName + "/" + predicateArity];
                // TODO: B0 should be set to B
            }
            else
            {
                state.Backtrack();
            }
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
