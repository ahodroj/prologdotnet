//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeMethodArgumentTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeMethodArgumentTest
	{
		[Test]
		public void TypePassingTest ()
		{
            PrologCodeMethodArgument prologCodeMethodArgument = new PrologCodeMethodArgument(1, 2);

            int result = prologCodeMethodArgument.Type;
            int result1 = prologCodeMethodArgument.Passing;

            Assert.AreEqual(result, 1);
            Assert.AreEqual(result1, 2);
		}
	}
}