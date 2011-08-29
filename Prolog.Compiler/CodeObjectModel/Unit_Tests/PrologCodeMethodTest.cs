//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeMethodTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeMethodTest
	{
		[Test]
		public void ClassTest ()
		{
            PrologCodeMethod prologCodeMethod = new PrologCodeMethod("Class1", "soo.dll", 1, "foo");

            string result = prologCodeMethod.Class;

            Assert.AreEqual(result, "Class1");
		}
		
		[Test]
		public void AssemblyNameTest ()
		{
            PrologCodeMethod prologCodeMethod = new PrologCodeMethod("Class1", "soo.dll", 1, "foo");

            string result = prologCodeMethod.AssemblyName;
		
			Assert.AreEqual(result, "soo.dll");
		}
		
		[Test]
		public void TypeTest ()
		{
            PrologCodeMethod prologCodeMethod = new PrologCodeMethod("Class1", "soo.dll", 1, "foo");

            int result = prologCodeMethod.Type;
		
			Assert.AreEqual(result, 1);
		}
		
		[Test]
		public void MethodNameTest ()
		{
            PrologCodeMethod prologCodeMethod = new PrologCodeMethod("Class1", "soo.dll", 1, "foo");

            string result = prologCodeMethod.MethodName;
		
			Assert.AreEqual(result, "foo");
		}
		
		[Test]
		public void PredicateNameTest ()
		{
            PrologCodeMethod prologCodeMethod = new PrologCodeMethod("Class1", "soo.dll", 1, "foo", "predicate");

            string result = prologCodeMethod.PredicateName;
		
			Assert.AreEqual(result, "predicate");
		}
	}
}