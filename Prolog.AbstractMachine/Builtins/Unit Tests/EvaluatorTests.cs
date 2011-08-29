

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;
using Axiom.Runtime.Builtins;
using Axiom.Runtime.Builtins.IO;
using Axiom.Runtime.Builtins.Equality;
using Axiom.Runtime.Builtins.Comparison.Numeric;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _TermEvaluator
    {

        public AbstractMachineState SetupMachine()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new HaltInstruction());

            state.Initialize(prog);

            return state;
        }

        
        public void eval(AbstractTerm term, double expected)
        {
            Assert.AreEqual(expected, TermEvaluator.Evaluate(term));
        }

        [Test]
        public void int_digit()
        {
            eval(new ConstantTerm("2"), 2);
        }

        [Test]
        public void float_digit()
        {
            eval(new ConstantTerm("1.23"), 1.23);
        }

        [Test]
        public void add_expr()
        {
            StructureTerm addOp = new StructureTerm("+", 2);
            addOp.Next = new ConstantTerm("1");
            addOp.Next.Next = new ConstantTerm("2");

            eval(addOp, 3);
        }

        [Test]
        public void sub_expr()
        {
            StructureTerm addOp = new StructureTerm("-", 2);
            addOp.Next = new ConstantTerm("5");
            addOp.Next.Next = new ConstantTerm("2");

            eval(addOp, 3);
        }

        [Test]
        public void mul_expr()
        {
            StructureTerm addOp = new StructureTerm("*", 2);
            addOp.Next = new ConstantTerm("5");
            addOp.Next.Next = new ConstantTerm("2");

            eval(addOp, 10);
        }

        [Test]
        public void div_expr()
        {
            StructureTerm addOp = new StructureTerm("/", 2);
            addOp.Next = new ConstantTerm("6");
            addOp.Next.Next = new ConstantTerm("2");

            eval(addOp, 6/2);
        }

        [Test]
        public void cos_expr()
        {
            StructureTerm addOp = new StructureTerm("cos", 1);
            addOp.Next = new ConstantTerm("5");
            

            eval(addOp, Math.Cos(5));
        }

        [Test]
        public void sin_expr()
        {
            StructureTerm addOp = new StructureTerm("sin", 1);
            addOp.Next = new ConstantTerm("5");


            eval(addOp, Math.Sin(5));
        }

        [Test]
        public void tan_expr()
        {
            StructureTerm addOp = new StructureTerm("tan", 1);
            addOp.Next = new ConstantTerm("5");


            eval(addOp, Math.Tan(5));
        }

        [Test]
        public void log_expr()
        {
            StructureTerm addOp = new StructureTerm("log", 1);
            addOp.Next = new ConstantTerm("5");


            eval(addOp, Math.Log(5));
        }

        [Test]
        public void pow_expr()
        {
            StructureTerm op = new StructureTerm("^", 2);
            op.Next = new ConstantTerm("3");
            op.Next.Next = new ConstantTerm("2");

            eval(op, Math.Pow(3, 2));
        }


    }
}