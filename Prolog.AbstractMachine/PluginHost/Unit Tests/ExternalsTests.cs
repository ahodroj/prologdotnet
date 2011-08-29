

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _ExternalsTests
    {

        AbstractPredicate _p = null;

        public AbstractMachineState SetupMachine()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new HaltInstruction());

            state.Initialize(prog);

            return state;
        }

        public void Verify(string name, int arity)
        {
            Assert.AreEqual(name, _p.Name());
            Assert.AreEqual(arity, _p.Arity());
        }


        [Test]
        public void asserta_1()
        {
            AMPredicateSet set = AMPredicateSet.Instance;

            AbstractMachineState state = SetupMachine();

            IAbstractMachinePredicate pred = set.CreatePredicate("asserta/1");

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            
            X0.Assign(new ConstantTerm("it_is_sunny"));

            Assert.AreEqual("asserta", pred.Name());
            Assert.AreEqual(1, pred.Arity());

            pred.Execute(state);

            AMProgram program = (AMProgram)state.Program;

            Assert.IsNotNull(program["it_is_sunny/0"]);
        }

        [Test]
        public void assertz_1()
        {
            AMPredicateSet set = AMPredicateSet.Instance;

            AbstractMachineState state = SetupMachine();

            IAbstractMachinePredicate pred = set.CreatePredicate("assertz/1");

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("it_is_sunny"));

            Assert.AreEqual("assertz", pred.Name());
            Assert.AreEqual(1, pred.Arity());

            pred.Execute(state);

            AMProgram program = (AMProgram)state.Program;

            Assert.IsNotNull(program["it_is_sunny/0"]);
        }
    }
}