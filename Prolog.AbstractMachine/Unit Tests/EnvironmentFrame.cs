

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _EnvironmentFrame
    {

    	[Test]
    	public void Create() {
    		
    		EnvironmentFrame f = new EnvironmentFrame();
    		Assert.IsNull(f.CE);
    		Assert.IsNull(f.CP);
    		Assert.IsNull(f.Next);
    		Assert.IsNull(f.Previous);
    		Assert.IsNull(f.PermanentVariables);
    	}
    	
    	[Test]
    	public void CE_CP_Size() {
    		ProgramNode _cp = new ProgramNode();
    		EnvironmentFrame _ce = new EnvironmentFrame();
    		
    		EnvironmentFrame f = new EnvironmentFrame(_ce, _cp, 10);
    		
    		Assert.AreSame(_ce, f.CE);
    		Assert.AreSame(_cp, f.CP);
    		for(int i = 0; i < 10; i++) {
    			string varName = "Y" + i.ToString();
    			Assert.IsNotNull(f[varName]);
    		}
    	}
    
    	
    	[Test]
    	public void AddVariable() {
    		EnvironmentFrame f = new EnvironmentFrame();
    		
    		Assert.IsNull(f["Y0"]);
    		
    		f.AddVariable();
    		Assert.IsNotNull(f["Y0"]);
    		
    		Assert.IsNull(f["Y1"]);
    		
    		f.AddVariable();
    		Assert.IsNotNull(f["Y1"]);
    	}
    	
    	[Test]
    	public void PermanentVariables() {
    		EnvironmentFrame f = new EnvironmentFrame(null, null, 5);
    		
    		Assert.AreSame(f["Y0"], f.PermanentVariables);
    		Assert.AreSame(f["Y1"], f.PermanentVariables.Next);
    		Assert.AreSame(f["Y2"], f.PermanentVariables.Next.Next);
    		Assert.AreSame(f["Y3"], f.PermanentVariables.Next.Next.Next);
    		Assert.AreSame(f["Y4"], f.PermanentVariables.Next.Next.Next.Next);
    		Assert.IsNull(		    f.PermanentVariables.Next.Next.Next.Next.Next);
    		
    	
    	}
    	
    }
}
