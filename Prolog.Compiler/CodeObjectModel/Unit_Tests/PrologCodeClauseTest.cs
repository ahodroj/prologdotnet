//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeClauseTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeClauseTest
	{
		[Test]
		public void ArityTest()
		{
			PrologCodeClause prologCodeClause = new PrologCodeClause();
		
			int result = prologCodeClause.Arity;
		
			Assert.AreEqual(result, 0);
		}
	}
}