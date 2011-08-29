

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _Instructions
    {

        public AbstractMachineState SetupMachine()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new HaltInstruction());

            state.Initialize(prog);

            return state;
        }

        [Test]
        public void PutXVariable()
        {
            AbstractMachineState state = SetupMachine();

            PutVariableInstruction i = new PutVariableInstruction();

            object[] args = { "X0", "X1" };
           
            i.Process(args);
            i.Execute(state);

            Assert.AreSame(state["H"], ((AbstractTerm)state["X0"]).Dereference());
            Assert.AreSame(state["H"], ((AbstractTerm)state["X1"]).Dereference());
            Assert.AreEqual("put_variable", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            
        }

        [Test]
        public void PutYVariable()
        {
            AbstractMachineState state = SetupMachine();

            state.E = new EnvironmentFrame(null, null, 10);

            PutVariableInstruction i = new PutVariableInstruction();

            object[] args = { "Y0", "X0" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("put_variable", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            Assert.IsTrue(state.E["Y0"].IsReference);
            Assert.AreSame(state.E["Y0"].Dereference(), ((AbstractTerm)state["X0"]).Dereference());

        }



        [Test]
        public void PutValue()
        {
            AbstractMachineState state = SetupMachine();

            PutValueInstruction i = new PutValueInstruction();

            object[] args = { "X0", "X1" };

            i.Process(args);
            AbstractTerm X0 = (AbstractTerm)state["X0"];
            X0.Assign(new ConstantTerm("ali"));

            i.Execute(state);


            AbstractTerm X1 = (AbstractTerm)state["X1"];
            Assert.AreEqual("ali", X1.Data());
            Assert.AreEqual("put_value", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
        }


        [Test]
        public void PutUnsafeValue()
        {
            AbstractMachineState state = SetupMachine();

            state.E = new EnvironmentFrame(null, null, 3);

            

        }

        [Test]
        public void PutStructure()
        {
            AbstractMachineState state = SetupMachine();

            PutStructureInstruction i = new PutStructureInstruction();

            Assert.AreEqual("put_structure", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());

            object[] args = { "s/2", "X0" };

            i.Process(args);

            i.Execute(state);

            AbstractTerm H = (AbstractTerm)state["H"];

            Assert.IsTrue(H.IsStructure);
            Assert.AreEqual(2, H.Arity);
            Assert.AreEqual("s", H.Name);
            
            AbstractTerm X0 = (AbstractTerm)state["X0"];
            Assert.AreSame(H.Dereference(), X0.Dereference());

            Assert.IsTrue(X0.IsStructure);
            Assert.AreEqual(2, X0.Arity);
            Assert.AreEqual("s", X0.Name);
            
            
        }

        [Test]
        public void PutList()
        {
            AbstractMachineState state = SetupMachine();

            PutListInstruction i = new PutListInstruction();

            object[] args = { "X0" };

            i.Process(args);

            Assert.AreEqual("put_list", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());

            i.Execute(state);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            Assert.IsTrue(X0.IsList);
            
        }

        [Test]
        public void PutConstant()
        {
            AbstractMachineState state = SetupMachine();

            PutConstantInstruction i = new PutConstantInstruction();

            object[] args = { "ali", "X0" };

            i.Process(args);

            Assert.AreEqual("put_constant", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());

            i.Execute(state);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            Assert.IsTrue(X0.IsConstant);
            Assert.AreEqual("ali", X0.Data());

        }

        [Test]
        public void GetVariable()
        {
            AbstractMachineState state = SetupMachine();

            GetVariableInstruction i = new GetVariableInstruction();

            object[] args = { "X1", "X0" };

            i.Process(args);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            X0.Assign(new ConstantTerm("ali"));

            i.Execute(state);

            Assert.AreEqual("get_variable", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            Assert.AreEqual(X1.Data(), "ali");
        }

        [Test]
        public void GetValue()
        {
            AbstractMachineState state = SetupMachine();

            GetValueInstruction i = new GetValueInstruction();

            object[] args = { "X1", "X0" };

            i.Process(args);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];

            X0.Assign(new ConstantTerm("ali"));

            i.Execute(state);

            Assert.AreEqual("get_value", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            Assert.AreEqual(X1.Data(), "ali");
        }

        [Test]
        public void GetStructure()
        {
            AbstractMachineState state = SetupMachine();

            GetStructureInstruction i = new GetStructureInstruction();

            object[] args = { "s/2", "X0" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("get_structure", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            Assert.AreEqual("s", X0.Name);
            Assert.AreEqual(2, X0.Arity);
        }

        [Test]
        public void GetList()
        {
            AbstractMachineState state = SetupMachine();

            GetListInstruction i = new GetListInstruction();

            object[] args = { "X0" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("get_list", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            Assert.IsTrue(X0.IsList);

        }

        [Test]
        public void GetConstant()
        {
            AbstractMachineState state = SetupMachine();

            GetConstantInstruction i = new GetConstantInstruction();

            object[] args = { "ali", "X0" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("get_constant", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            Assert.IsTrue(X0.IsConstant);
            Assert.AreEqual("ali", X0.Data());
        }

        [Test]
        public void SetVariable()
        {
            AbstractMachineState state = SetupMachine();

            SetVariableInstruction i = new SetVariableInstruction();

            object[] args = { "X0" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("set_variable", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();

            AMHeap heap = (AMHeap)state.DataArea;

            Assert.AreSame(X0, heap.Top());
        }

        [Test]
        public void SetValue()
        {
            AbstractMachineState state = SetupMachine();
            

            SetValueInstruction i = new SetValueInstruction();

            object[] args = { "X0" };

            i.Process(args);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            i.Execute(state);

            Assert.AreEqual("set_value", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreSame(X0.Dereference(), ((AbstractTerm)state["H"]).Dereference());
        }

        [Test]
        public void SetLocalValue()
        {
            AbstractMachineState state = SetupMachine();

            EnvironmentFrame env = new EnvironmentFrame(null, null, 3);
            state.E = env;

            SetLocalValueInstruction i = new SetLocalValueInstruction();

            object[] args = { "X0" };

            
            i.Process(args);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            i.Execute(state);

            Assert.AreEqual("set_local_value", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreSame(X0.Dereference(), ((AbstractTerm)state["H"]).Dereference());
        }

        [Test]
        public void SetConstant()
        {
            AbstractMachineState state = SetupMachine();
            
            SetConstantInstruction i = new SetConstantInstruction();

            object[] args = { "ali" };

            i.Process(args);
            i.Execute(state);

            AbstractTerm H = (AbstractTerm)((AMHeap)state.DataArea).Top();


            Assert.AreEqual("set_constant", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreEqual("ali", H.Data());
        }

        [Test]
        public void SetVoid()
        {
            AbstractMachineState state = SetupMachine();
            AMHeap heap = (AMHeap)state.DataArea;


            object[] args = { "3" };

            SetVoidInstruction i = new SetVoidInstruction();

            i.Process(args);
            i.Execute(state);

            Assert.AreEqual("set_void", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
        }

        [Test]
        public void UnifyVariable()
        {
            AbstractMachineState state = SetupMachine();

            state.IsReadMode = true;

            AbstractTerm sVar = new AbstractTerm();
            state.S = sVar;

            UnifyVariableInstruction i = new UnifyVariableInstruction();

            object[] args = { "X0" };

            i.Process(args);
            i.Execute(state);

            Assert.AreEqual("unify_variable", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            Assert.AreSame(X0.Dereference(), sVar);
            Assert.IsNull(state.S);
        }

        [Test]
        public void UnifyLocalValue()
        {
            AbstractMachineState state = SetupMachine();

            UnifyLocalValueInstruction i = new UnifyLocalValueInstruction();

            ConstantTerm con = new ConstantTerm("ali");
            state.S = con;

            object[] args = { "X0" };

            i.Process(args);
            i.Execute(state);

            Assert.AreEqual("unify_local_value", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            AbstractTerm  X0 = (AbstractTerm)state["X0"];
            Assert.AreEqual("ali", X0.Dereference().Data());
           

        }

        [Test]
        public void UnifyValue()
        {
            AbstractMachineState state = SetupMachine();

            UnifyValueInstruction i = new UnifyValueInstruction();

            object[] args = { "X0" };


            state.S = new ConstantTerm("ali");

            i.Process(args);
            i.Execute(state);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            Assert.AreEqual("unify_value", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreEqual("ali", X0.Data());   
        }

        [Test]
        public void UnifyConstant()
        {
            AbstractMachineState state = SetupMachine();

            UnifyConstantInstruction i = new UnifyConstantInstruction();

            object[] args = { "ali" };

            state.S = new AbstractTerm();

            i.Process(args);
            i.Execute(state);

            Assert.AreEqual("unify_constant", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreEqual("ali", ((AbstractTerm)state.S).Data());
        }

        [Test]
        public void UnifyVoid()
        {
            AbstractMachineState state = SetupMachine();
            AMHeap heap = (AMHeap)state.DataArea;

            UnifyVoidInstruction i = new UnifyVoidInstruction();

            state.IsWriteMode = true;

            object[] args = { "3" };

            i.Process(args);
            i.Execute(state);

            Assert.AreEqual("unify_void", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
            Assert.IsTrue(((AbstractTerm)heap.Pop()).IsReference);
        }

        [Test]
        public void Allocate()
        {
            AbstractMachineState state = SetupMachine();

            AMHeap heap = (AMHeap)state.DataArea;

            AllocateInstruction i = new AllocateInstruction();

            i.Process(null);
            i.Execute(state);


            EnvironmentFrame env = (EnvironmentFrame)heap.Top();

            Assert.AreEqual("allocate", i.Name());
            Assert.AreEqual(0, i.NumberOfArguments());
            Assert.AreSame(env, state.E);
            for (int r = 0; r < 20; r++)
            {
                Assert.IsNotNull(env["Y" + r.ToString()]);
            }
        }

        [Test]
        public void Deallocate()
        {
            AbstractMachineState state = SetupMachine();
            AMProgram program = (AMProgram)state.Program;


            ProgramNode CP = new ProgramNode();
            EnvironmentFrame env = new EnvironmentFrame();
            state.E = new EnvironmentFrame(env, CP, 2);

            

            DeallocateInstruction i = new DeallocateInstruction();

            i.Execute(state);

            Assert.AreEqual("deallocate", i.Name());
            Assert.AreEqual(0, i.NumberOfArguments());
            Assert.AreSame(CP, program.CP);
            Assert.AreSame(env, state.E);

        }

        [Test]
        public void Call()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new PutConstantInstruction());
            prog.Add(new HaltInstruction());
            
            
            state.Initialize(prog);

            AMProgram program = (AMProgram)state.Program;
           
            ProgramClause clause = new ProgramClause("male", 2);

            program.AddLabel("male/2", clause);

            CallInstruction i = new CallInstruction();

            object[] args = { "male", "2" };

            i.Process(args);

            i.Execute(state);
            Assert.AreEqual("call", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            Assert.AreSame(clause, program.P);
            Assert.AreEqual("halt", program.CP.Instruction.Name());
        }

        [Test]
        public void Execute()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new PutConstantInstruction());
            prog.Add(new HaltInstruction());


            state.Initialize(prog);

            AMProgram program = (AMProgram)state.Program;

            ProgramClause clause = new ProgramClause("male", 2);

            program.AddLabel("male/2", clause);

            ExecuteInstruction i = new ExecuteInstruction();

            object[] args = { "male", "2" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("execute", i.Name());
            Assert.AreEqual(2, i.NumberOfArguments());
            Assert.AreSame(clause, program.P);
               
        }
        [Test]
        public void Proceed()
        {
            AbstractMachineState state = SetupMachine();

            AMProgram program = (AMProgram)state.Program;

            program.CP = new ProgramNode();

            ProceedInstruction i = new ProceedInstruction();

            i.Process(null);

            i.Execute(state);

            Assert.AreEqual("proceed", i.Name());
            Assert.AreEqual(0, i.NumberOfArguments());
            Assert.AreSame(program.CP, program.P);
        }

        [Test]
        public void TryMeElse()
        {
            AbstractMachineState state = SetupMachine();

            AMProgram program = (AMProgram)state.Program;
            AMTrail trail = (AMTrail)state.Trail;

            program.AddLabel("foobar/2", new ProgramClause());

            TryMeElseInstruction i = new TryMeElseInstruction();

            object[] args = { "foobar/2" };

            i.Process(args);

            program.NumberOfArguments = 2;

            i.Execute(state);

            

            Assert.AreEqual("try_me_else", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreEqual(2, state.B.Arity);
            Assert.IsNull(state.B.B);
            Assert.AreSame(state.B.CE, state.E);
            Assert.AreSame(state.B.CP, program.CP);
            Assert.AreSame(state.B.NextClause, program["foobar/2"]);
            Assert.AreEqual(state.B.TR, trail.TR);
        }

        [Test]
        public void RetryMeElse()
        {
            AbstractMachineState state = SetupMachine();

            RetryMeElseInstruction i = new RetryMeElseInstruction();

            AMProgram program = (AMProgram)state.Program;
            AMTrail trail = (AMTrail)state.Trail;


            Choicepoint b = new Choicepoint();
            b.CE = new EnvironmentFrame();
            b.B = new Choicepoint();
            b.CP = new ProgramNode();
            b.TR = 1;
            b.NextClause = new ProgramClause();
            state.B = b;

            program.AddLabel("foobar/2", new ProgramClause());

            object[] args = { "foobar/2" };

            i.Process(args);

            i.Execute(state);

            Assert.AreEqual("retry_me_else", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            Assert.AreSame(state.E, b.CE);
            Assert.AreEqual(b.TR, trail.TR);
            Assert.AreEqual(state.B.NextClause, program["foobar/2"]);
        }

        [Test]
        public void TrustMe()
        {
            AbstractMachineState state = SetupMachine();

            TrustMeInstruction i = new TrustMeInstruction();

            i.Process(null);

            AMProgram program = (AMProgram)state.Program;
            AMTrail trail = (AMTrail)state.Trail;


            Choicepoint b = new Choicepoint();
            b.CE = new EnvironmentFrame();
            Choicepoint old = new Choicepoint();
            b.B = old;
            b.CP = new ProgramNode();
            b.TR = 1;
            b.NextClause = new ProgramClause();
            state.B = b;


            i.Execute(state);

            Assert.AreEqual("trust_me", i.Name());
            Assert.AreEqual(0, i.NumberOfArguments());
            Assert.AreSame(state.E, b.CE);
            Assert.AreSame(program.CP, b.CP);
            Assert.AreEqual(b.TR, trail.TR);
            Assert.AreSame(state.B, old);
        }

        [Test]
        public void Cut()
        {
            AbstractMachineState state = SetupMachine();

            Choicepoint old = new Choicepoint();

            state.E = new EnvironmentFrame(null, null, old, 0);

            CutInstruction i = new CutInstruction();

            i.Process(null);

            i.Execute(state);

            Assert.AreEqual("cut", i.Name());
            Assert.AreEqual(0, i.NumberOfArguments());
            Assert.AreSame(state.B, old);
        }

        [Test]
        public void BCall()
        {
            AbstractMachineState state = SetupMachine();


            BCallInstruction i = new BCallInstruction();

            i.Process(new string[] { "write/1" });

            Assert.AreEqual("bcall", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());
            
        }

        [Test]
        public void FCall()
        {
            AbstractMachineState state = SetupMachine();

            FCallInstruction i = new FCallInstruction();

            Assert.AreEqual("fcall", i.Name());
            Assert.AreEqual(4, i.NumberOfArguments());
            
        }

        [Test]
        public void CallVar()
        {
            AbstractMachineState state = SetupMachine();

            CallVariableInstruction i = new CallVariableInstruction();

            i.Process(new string [] { "X0" });

            AMProgram program = (AMProgram)state.Program;

            ProgramClause x = new ProgramClause("male", 1);

            program.AddLabel("male/1", x);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            StructureTerm s = new StructureTerm("male", 1);
            s.Next = new ConstantTerm("ali");

            X0.Assign(s);

            i.Execute(state);


            Assert.AreSame(program.P, x);
            Assert.AreEqual("callvar", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());

        }

        [Test]
        public void ExecuteVar()
        {
            AbstractMachineState state = SetupMachine();

            ExecuteVariableInstruction i = new ExecuteVariableInstruction();

            i.Process(new string[] { "X0" });

            AMProgram program = (AMProgram)state.Program;

            ProgramClause x = new ProgramClause("male", 1);

            program.AddLabel("male/1", x);

            AbstractTerm X0 = (AbstractTerm)state["X0"];

            StructureTerm s = new StructureTerm("male", 1);
            s.Next = new ConstantTerm("ali");

            X0.Assign(s);

            i.Execute(state);


            Assert.AreSame(program.P, x);
            Assert.AreEqual("executevar", i.Name());
            Assert.AreEqual(1, i.NumberOfArguments());

        }
    }

    public class ExamplePredicate : AbstractPredicate
    {
        public override int Arity()
        {
            return 1;
        }

        public override string Name()
        {
            return "Example";
        }

        public override void Execute(AbstractMachineState state)
        {
            state.B = null;
        }
    }
}