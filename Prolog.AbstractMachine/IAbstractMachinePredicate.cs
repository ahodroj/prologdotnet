using System;

namespace Axiom.Runtime
{
    /// <summary>
    /// provides an interface to implement an abstract machine predicate.
    /// </summary>
    public interface IAbstractMachinePredicate
    {
        /// <summary>
        /// Predicate name
        /// </summary>
        string Name();

        /// <summary>
        /// Predicate arity
        /// </summary>
        int Arity();

        /// <summary>
        /// Executes the predicate.
        /// </summary>
        /// <param name="state">An instance of an abstract machine</param>
        void Execute(AbstractMachineState state);

    }
}
