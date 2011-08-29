//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodePredicateTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodePredicateTest
	{
		[Test]
		public void ArgumentsTest ()
		{
            PrologCodePredicate prologCodePredicate = new PrologCodePredicate("predicate");

            ArrayList result = prologCodePredicate.Arguments;

            Assert.AreEqual(0, result.Count);
		}
		
		[Test]
		public void ArityTest ()
		{
            PrologCodePredicate prologCodePredicate = new PrologCodePredicate();

            int result = prologCodePredicate.Arity;
		
			Assert.AreEqual(0, result);
		}
		
		[Test]
		public void NameTest ()
		{
            PrologCodePredicate prologCodePredicate = new PrologCodePredicate("male");

            string result = prologCodePredicate.Name;
		
			Assert.AreEqual("male", result);
		}
	}
}