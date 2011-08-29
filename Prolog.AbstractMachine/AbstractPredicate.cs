using System;
using System.Collections;


namespace Axiom.Runtime
{
    /// <summary>
    /// Provides an interface to implement builtin predicates.
    /// </summary>
    public abstract class AbstractPredicate : IAbstractMachinePredicate
    {
        public abstract void Execute(AbstractMachineState state);
        public abstract string Name();
        public abstract int Arity();
    }
}
