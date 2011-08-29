//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeUnitTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeUnitTest
	{
		[Test]
		public void MethodsTest ()
		{
            PrologCodeUnit prologCodeUnit = new PrologCodeUnit();

            ArrayList result = prologCodeUnit.Methods;
		
			Assert.AreEqual(0, result.Count);
		}
		
		[Test]
		public void OperatorsTest ()
		{
            PrologCodeUnit prologCodeUnit = new PrologCodeUnit();

            ArrayList result = prologCodeUnit.Operators;
		
			Assert.AreEqual(0, result.Count);
		}
		
		[Test]
		public void AssemblyNameTest ()
		{
            PrologCodeUnit prologCodeUnit = new PrologCodeUnit();

            prologCodeUnit.AssemblyName = "System.dll";

            string result = prologCodeUnit.AssemblyName;
		
			Assert.AreEqual("System.dll", result);
		}
		
		[Test]
		public void ClassTest ()
		{
            PrologCodeUnit prologCodeUnit = new PrologCodeUnit();
            prologCodeUnit.Class = "Class1";
            string result = prologCodeUnit.Class;
		
			Assert.AreEqual("Class1", result);
		}
	}
}