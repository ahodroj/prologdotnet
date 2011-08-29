

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AbstractMachineState
    {

        [Test]
        public void Initialize()
        {
            ArrayList prog = new ArrayList();
            prog.Add(new NopInstruction());
            prog.Add(new HaltInstruction());

            AbstractMachineState state = new AbstractMachineState(new AMFactory());

            state.Initialize(prog);

            Assert.IsNotNull(state.Program);
            Assert.IsNotNull(state.DataArea);

        }

        [Test]
        public void Stop()
        {
            ArrayList prog = new ArrayList();
            prog.Add(new NopInstruction());
            prog.Add(new HaltInstruction());

            AbstractMachineState state = new AbstractMachineState(new AMFactory());

            state.Initialize(prog);

            Assert.IsFalse(state.Stop());
        }

        [Test]
        public void Transition()
        {
            ArrayList prog = new ArrayList();
            prog.Add(new NopInstruction());
            prog.Add(new HaltInstruction());

            AbstractMachineState state = new AbstractMachineState(new AMFactory());

            state.Initialize(prog);
            state.Transition();

            Assert.IsTrue(state.Stop());

        }

        [Test]
        public void Backtrack()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            AMProgram program = (AMProgram)state.Program;

            state.Backtrack();
            Assert.IsNotNull(program.P);

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(2, null, null, null, nextClause, 3, null);

            state.Backtrack();

            Assert.AreSame(program.P, nextClause);
        }

        [Test]
        public void X_Registers()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());

        }

        [Test]
        public void Call()
        {
            ArrayList prog = new ArrayList();
            prog.Add(new NopInstruction());
            prog.Add(new HaltInstruction());

            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            state.Initialize(prog);

            AMProgram program = (AMProgram)state.Program;

            ArrayList predicateCode = new ArrayList();

            AMInstructionSet iset = new AMInstructionSet();

            // say_hello(X) :- write(X).

            predicateCode.Add(iset.CreateInstruction("bcall", "write/1"));
            predicateCode.Add(iset.CreateInstruction("proceed"));

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("Hello, World!"));

            program.AssertFirst("say_hello", 1, predicateCode);

            Assert.IsTrue(state.Call("say_hello", 1, new object[] { "Hello man" }));
        }
    }
}