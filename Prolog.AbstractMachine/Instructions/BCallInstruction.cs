using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Axiom.Runtime.Instructions
{
    public class BCallInstruction : AbstractInstruction
    {
        private string _builtinName;
   
        public override string Name()
        {
            return "bcall";
        }

        public override int NumberOfArguments()
        {
            return 1;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMPredicateSet pset = AMPredicateSet.Instance;

            if (pset.IsValidPredicate(_builtinName))
            {
                IAbstractMachinePredicate p = (IAbstractMachinePredicate)pset.CreatePredicate(_builtinName);
                // determine if p is a non-deterministic predicate
                Type pType = p.GetType();
                if (pType.IsInstanceOfType(new AbstractNonDeterministicPredicate()))
                {
                    ((AbstractNonDeterministicPredicate)p).IncrementCallID();
                }
                p.Execute(state);
            }

        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _builtinName = (string)arguments[0];
        }


        public override string ToString()
        {
            return Name() + " " + _builtinName;
        }
    }
}
