

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;
using Axiom.Runtime.Builtins.IO;
using Axiom.Runtime.Builtins.Equality;
using Axiom.Runtime.Builtins.Comparison.Numeric;
using Axiom.Runtime.Builtins.Meta;
using Axiom.Runtime.Builtins.Control;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _Builtins
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
        public void write_1()
        {
            AbstractMachineState state = SetupMachine();

            _p = new WritePredicate();

            Verify("write", 1);

            _p.Execute(state);

        }

        [Test]
        public void writeln_1()
        {
            AbstractMachineState state = SetupMachine();

            _p = new WriteLnPredicate();

            Verify("writeln", 1);

            _p.Execute(state);

        }

        [Test]
        public void nl_0()
        {
            AbstractMachineState state = SetupMachine();

            _p = new NlPredicate();

            Verify("nl", 0);

            _p.Execute(state);

        }

        [Test]
        public void get0_1()
        {
            AbstractMachineState state = SetupMachine();

            _p = new Get0Predicate();

            Verify("get0", 1);

            _p.Execute(state);

        }

        [Test]
        public void skip_1()
        {
            AbstractMachineState state = SetupMachine();

            _p = new SkipPredicate();

            Verify("skip", 1);

            _p.Execute(state);

        }

        [Test]
        public void put_1()
        {
            AbstractMachineState state = SetupMachine();

            _p = new PutPredicate();

            AbstractTerm x0 = (AbstractTerm)state["X0"];

            x0.Assign(new ConstantTerm("61"));

            Verify("put", 1);

            _p.Execute(state);

        }

        // Comparison tests
        [Test]
        public void equals_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new EqualsPredicate();

            Verify("=:=", 2);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);

            X0.Assign(new ConstantTerm("3"));
            X1.Assign(new ConstantTerm("5"));


            // X0 == X1
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void greaterthanequals_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new GreaterThanEqualPredicate();

            Verify(">=", 2);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);

            X0.Assign(new ConstantTerm("3"));
            X1.Assign(new ConstantTerm("5"));


            // X0 == X1
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);

        }


        [Test]
        public void greaterthan_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new GreaterThanPredicate();

            Verify(">", 2);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);

            X0.Assign(new ConstantTerm("3"));
            X1.Assign(new ConstantTerm("5"));


            // X0 == X1
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void lessthanequals_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new LessThanEqualPredicate();

            Verify("=<", 2);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);

            X0.Assign(new ConstantTerm("5"));
            X1.Assign(new ConstantTerm("1"));


            // X0 == X1
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);

        }

        [Test]
        public void lessthan_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new LessThanPredicate();

            Verify("<", 2);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);

            X0.Assign(new ConstantTerm("5"));
            X1.Assign(new ConstantTerm("1"));


            // X0 < X1
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);

        }

        // Control tests
        [Test]
        public void call_1()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new CallPredicate();

            Verify("call", 1);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            StructureTerm goal = new StructureTerm("male", 1);
            goal.Next = new ConstantTerm("ali");
            program.AddLabel("male/1", new ProgramClause("male", 1));

            X0.Assign(goal);

            _p.Execute(state);

            Assert.AreEqual(X0.Data(), "ali");
            ProgramClause p = (ProgramClause)program.P;
            Assert.AreEqual(p.Name, "male");
            Assert.AreEqual(p.Arity, 1);

        }

        [Test]
        public void unify_2()
        {
            AbstractMachineState state = SetupMachine();

            _p = new UnifyPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            X1.Assign(new ConstantTerm("ali"));

            Verify("=", 2);

            _p.Execute(state);

            Assert.AreEqual(X0.Data(), X1.Data());
            Assert.AreEqual("ali", X0.Data());

        }


        [Test]
        public void notunifiable_2()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new NotUnifiablePredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            X0.Assign(new ConstantTerm("ali"));
            X1.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();

            ProgramClause nextClause = new ProgramClause();

            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);
            Verify("\\=", 2);

            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
            
        }


        // meta tests
        [Test]
        public void atom_1_int()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new AtomPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            // integer
            X0.Assign(new ConstantTerm("10"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);
            
            
            Verify("atom", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void atom_1_atom()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new AtomPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            // integer
            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("atom", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }

        [Test]
        public void atom_1_string()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new AtomPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

    
            X0.Assign(new ConstantTerm("'Hello, World!"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("atom", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }

        [Test]
        public void atom_1_list()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new AtomPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];


            X0.Assign(new ListTerm());

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("atom", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void atom_1_struct()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new AtomPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];


            X0.Assign(new StructureTerm("f", 1));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("atom", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void bound_1_bound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new BoundPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("bound", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }

        [Test]
        public void bound_1_unbound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new BoundPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("bound", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void char_1_char()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new CharPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("a"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("char", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }

        [Test]
        public void char_1_nochar()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new CharPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("char", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }


        [Test]
        public void free_1_bound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new FreePredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("free", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void free_1_unbound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new FreePredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("free", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }


        [Test]
        public void integer_1_noint()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new IntegerPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("integer", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void integer_1_int()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new IntegerPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("32"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("integer", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }
        [Test]
        public void nonvar_1_bound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new NonVarPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("nonvar", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }

        [Test]
        public void nonvar_1_unbound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new NonVarPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];



            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("nonvar", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }


        [Test]
        public void string_1_string()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new CharPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("a"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("char", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }


        [Test]
        public void var_1_bound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new VarPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            X0.Assign(new ConstantTerm("ali"));

            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("var", 1);
            _p.Execute(state);

            Assert.AreSame(nextClause, program.P);
        }

        [Test]
        public void var_1_unbound()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;

            _p = new VarPredicate();

            AbstractTerm X0 = (AbstractTerm)state["X0"];



            Choicepoint b = new Choicepoint();
            ProgramClause nextClause = new ProgramClause();
            state.B = new Choicepoint(0, null, null, b, nextClause, 2, null);


            Verify("var", 1);
            _p.Execute(state);

            Assert.AreNotSame(nextClause, program.P);
        }
       
    }
}