//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeIntegerAtomTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeIntegerAtomTest
	{
		[Test]
		public void ValueTest ()
		{
            PrologCodeIntegerAtom prologCodeIntegerAtom = new PrologCodeIntegerAtom(123);

            int result = prologCodeIntegerAtom.Value;

            Assert.AreEqual(result, 123);
		}
	}
}