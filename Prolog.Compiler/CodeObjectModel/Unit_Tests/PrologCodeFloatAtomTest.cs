//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeFloatAtomTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeFloatAtomTest
	{
		[Test]
		public void ValueTest ()
		{
            PrologCodeFloatAtom prologCodeFloatAtom = new PrologCodeFloatAtom((float)1.2345);

            float result = prologCodeFloatAtom.Value;

            Assert.AreEqual(result, (float)1.2345);
		}
	}
}