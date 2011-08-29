//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeFactTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using System.Collections;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeFactTest
	{
		[Test]
		public void ArgumentsArityNameTest ()
		{
            PrologCodeFact prologCodeFact = new PrologCodeFact("male");

            ArrayList result = prologCodeFact.Arguments;
            int result1 = prologCodeFact.Arity;
            string result2 = prologCodeFact.Name;

            Assert.AreEqual(result1, 0);
            Assert.AreEqual(result2, "male");
		}
	}
}