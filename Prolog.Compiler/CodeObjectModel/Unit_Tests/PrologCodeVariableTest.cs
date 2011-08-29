//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeVariableTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeVariableTest
	{
		[Test]
		public void NameTest ()
		{
            PrologCodeVariable prologCodeVariable = new PrologCodeVariable("X");

            string result = prologCodeVariable.Name;
		
			Assert.AreEqual("X", result);
		}
	}
}