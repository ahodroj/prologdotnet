//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeStringAtomTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeStringAtomTest
	{
		[Test]
		public void ValueTest ()
		{
            PrologCodeStringAtom prologCodeStringAtom = new PrologCodeStringAtom("Hello, World!");

            string result = prologCodeStringAtom.Value;
		
			Assert.AreEqual("Hello, World!", result);
		}
	}
}