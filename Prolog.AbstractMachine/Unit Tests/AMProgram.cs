

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AMProgram
    {
        [Test]
        public void Initialize()
        {
            ArrayList program = new ArrayList();

            NopInstruction inst = new NopInstruction();
            program.Add(inst);

            AMProgram p = new AMProgram();
            p.Initialize(program);

            Assert.AreSame(p.Program.Instruction, inst);
        }

        [Test]
        public void Stop()
        {
            ArrayList p = new ArrayList();
            p.Add(new HaltInstruction());

            AMProgram program = new AMProgram();
            program.Initialize(p);

            Assert.IsTrue(program.Stop());
        }

        [Test]
        public void Next()
        {
            ArrayList p = new ArrayList();
            p.Add(new NopInstruction());
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();

            program.Initialize(p);

            program.Next();

            Assert.AreSame(program.P.Instruction, hi);
        }

        [Test]
        public void P()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            Assert.AreSame(program.P.Instruction, hi);
        }

        [Test]
        public void Program()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            Assert.AreSame(program.P.Instruction, program.Program.Instruction);
        }

        [Test]
        public void AddLabel_2()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));

            ProgramClause male1 = program["male/1"];
            ProgramClause male2 = male1.NextPredicate;

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "try_me_else");

            Assert.AreEqual(male2.Name, "male%1/1");
            Assert.AreEqual(male2.Arity, 1);
            Assert.AreEqual(male2.Instruction.Name(), "trust_me");
        }

        [Test]
        public void AssertLast()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AssertLast("male", 1, p);
            program.AssertLast("male", 1, p);
           

            ProgramClause male1 = program["male/1"];
            ProgramClause male2 = male1.NextPredicate;

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "try_me_else");

            Assert.AreEqual(male2.Name, "male%1/1");
            Assert.AreEqual(male2.Arity, 1);
            Assert.AreEqual(male2.Instruction.Name(), "trust_me");
        }

        [Test]
        public void AddLabel_3()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));

            ProgramClause male1 = program["male/1"];
            ProgramClause male2 = male1.NextPredicate;
            ProgramClause male3 = male2.NextPredicate;

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "try_me_else");

            Assert.AreEqual(male2.Name, "male%1/1");
            Assert.AreEqual(male2.Arity, 1);
            Assert.AreEqual(male2.Instruction.Name(), "retry_me_else");

            Assert.AreEqual(male3.Name, "male%2/1");
            Assert.AreEqual(male3.Arity, 1);
            Assert.AreEqual(male3.Instruction.Name(), "trust_me");
        }


        [Test]
        public void AddLabel_4()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));
            program.AddLabel("male/1", new ProgramClause("male", 1));

            ProgramClause male1 = program["male/1"];
            ProgramClause male2 = male1.NextPredicate;
            ProgramClause male3 = male2.NextPredicate;
            ProgramClause male4 = male3.NextPredicate;

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "try_me_else");

            Assert.AreEqual(male2.Name, "male%1/1");
            Assert.AreEqual(male2.Arity, 1);
            Assert.AreEqual(male2.Instruction.Name(), "retry_me_else");

            Assert.AreEqual(male3.Name, "male%2/1");
            Assert.AreEqual(male3.Arity, 1);
            Assert.AreEqual(male3.Instruction.Name(), "retry_me_else");

            Assert.AreEqual(male4.Name, "male%3/1");
            Assert.AreEqual(male4.Arity, 1);
            Assert.AreEqual(male4.Instruction.Name(), "trust_me");
        }



        [Test]
        public void AssertFirst_1()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AssertFirst("male", 1, p);

            ProgramClause male1 = program["male/1"];
           

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "nop"); 
        }

        [Test]
        public void AssertFirst_2()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AssertFirst("male", 1, p);


            ProgramClause oldFirst = program["male/1"];

            program.AssertFirst("male", 1, p);

            ProgramClause newFirst = program["male/1"];

            Assert.AreEqual(newFirst.Name, "male");
            Assert.AreEqual(newFirst.Arity, 1);
            Assert.AreEqual(newFirst.Instruction.Name(), "try_me_else");

            Assert.AreEqual(oldFirst.Name, "male%1/1");
            Assert.AreEqual(oldFirst.Arity, 1);
            Assert.AreEqual(oldFirst.Instruction.Name(), "trust_me");
        }

        [Test]
        public void AssertFirst_3()
        {
            ArrayList p = new ArrayList();
            HaltInstruction hi = new HaltInstruction();
            p.Add(hi);

            AMProgram program = new AMProgram();
            program.Initialize(p);

            program.AssertFirst("male", 1, p);


            ProgramClause male3 = program["male/1"];

            program.AssertFirst("male", 1, p);

            ProgramClause male2 = program["male/1"];

            program.AssertFirst("male", 1, p);

            ProgramClause male1 = program["male/1"];

            Assert.AreEqual(male1.Name, "male");
            Assert.AreEqual(male1.Arity, 1);
            Assert.AreEqual(male1.Instruction.Name(), "try_me_else");

            Assert.AreEqual(male2.Name, "male%1/1");
            Assert.AreEqual(male2.Arity, 1);
            Assert.AreEqual(male2.Instruction.Name(), "retry_me_else");

            Assert.AreEqual(male3.Name, "male%2/1");
            Assert.AreEqual(male3.Arity, 1);
            Assert.AreEqual(male3.Instruction.Name(), "trust_me");

           
            
        }
        
    }
}