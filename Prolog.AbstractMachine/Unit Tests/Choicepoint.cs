

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _Choicepoint
    {
    	[Test]
    	public void Create() {
    		Choicepoint c = new Choicepoint();
    		
    		Assert.IsNull(c.B);
    		Assert.IsNull(c.CE);
    		Assert.IsNull(c.CP);
    		Assert.IsNull(c.H);
    		Assert.IsNull(c.NextClause);
    	
    	}
    	
    	[Test]
    	public void Custom() {
    		HeapNode h = new HeapNode();
    		EnvironmentFrame ce = new EnvironmentFrame();
    		ProgramClause clause = new ProgramClause();
    		Choicepoint b = new Choicepoint();
    		ProgramNode cp = new ProgramNode();
    		
    		Choicepoint c = new Choicepoint(2, ce, cp, b, clause, 3, h);
    		
    		Assert.AreSame(h, c.H);
    		Assert.AreEqual(2, c.Arity);
    		Assert.AreSame(ce, c.CE);
    		Assert.AreSame(cp, c.CP);
    		Assert.AreSame(b, c.B);
    		Assert.AreSame(clause, c.NextClause);
    		Assert.AreEqual(3, c.TR);
    	}

        [Test]
        public void Indexing()
        {
            Choicepoint c = new Choicepoint();

            Assert.IsNull(c["Y5"]);

            
        }

    	[Test]
    	public void SaveVariable() {
    		Choicepoint c = new Choicepoint();
    		AbstractTerm term = new AbstractTerm();
    		
    		c.SaveVariable(term);

            Assert.AreSame(c["X0"], term);

            c["X0"].Assign(new ConstantTerm("ali"));

            Assert.AreEqual("ali", c["X0"].Data());
    	
    	}

        [Test]
        public void SaveRegisters()
        {
            AbstractMachineState state = new AbstractMachineState(new AMFactory());
            ArrayList prog = new ArrayList();
            prog.Add(new HaltInstruction());
            state.Initialize(prog);

            AbstractTerm X0 = (AbstractTerm)state["X0"];
            AbstractTerm X1 = (AbstractTerm)state["X1"];
            AbstractTerm X2 = (AbstractTerm)state["X2"];

            X0.Assign(new ConstantTerm("ali"));
            X1.Assign(new ConstantTerm("samir"));
            X2.Assign(new ConstantTerm("moe"));

            Choicepoint c = new Choicepoint();

            c.SaveRegisters(state, 3);

            Assert.AreEqual("ali", c["X0"].Data());
            Assert.AreEqual("samir", c["X1"].Data());
            Assert.AreEqual("moe", c["X2"].Data());
        }
    	
    	[Test]
    	public void H() {
    		Choicepoint c = new Choicepoint();
    		
    		HeapNode h = new HeapNode();
    		
    		c.H = h;
    		
    		Assert.AreSame(h, c.H);
    	}
    	
    	[Test]
    	public void CE() {
    		Choicepoint c = new Choicepoint();
    		
    		EnvironmentFrame ce = new EnvironmentFrame();
    		
    		c.CE = ce;
    		
    		Assert.AreSame(ce, c.CE);
    	}
    	
    	[Test]
    	public void CP() {
    		Choicepoint c = new Choicepoint();
    		
    		ProgramNode cp = new ProgramNode();
    		
    		c.CP = cp;
    		
    		Assert.AreSame(cp, c.CP);
    	}
    	
    	[Test]
    	public void B() {
    		Choicepoint c = new Choicepoint();
    		
    		Choicepoint b = new Choicepoint();
    		
    		c.B = b;
    		
    		Assert.AreSame(b, c.B);
    	}
    	
    	[Test]
    	public void NextClause() {
    		Choicepoint c = new Choicepoint();
    		
    		ProgramClause clause = new ProgramClause();
    		
    		c.NextClause = clause;
    		
    		Assert.AreSame(clause, c.NextClause);
    	}
    	
    	[Test]
    	public void TR() {
    		Choicepoint c = new Choicepoint();
    		
    		
    		
    		c.TR = 1;
    		
    		Assert.AreEqual(1, c.TR);
    	}
    
    	
    	[Test]
    	public void Arity() {
    		Choicepoint c = new Choicepoint();
    		
    		c.Arity = 4;
    		
    		Assert.AreEqual(4, c.Arity);
    	}
    }
}
