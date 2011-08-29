//..begin "File Description"
/*--------------------------------------------------------------------------------*
   Filename:  PrologCodeNonEmptyListTest.cs
   Tool:      objectiF, CSharpSSvr V6.0.82
 *--------------------------------------------------------------------------------*/
//..end "File Description"

using System;
using NUnit.Framework;

namespace Axiom.Compiler.CodeObjectModel.Unit_Tests
{	
	[TestFixture]
	public class PrologCodeNonEmptyListTest
	{
		[Test]
		public void AppendTest ()
		{
			// PrologCodeNonEmptyList prologCodeNonEmptyList = new PrologCodeNonEmptyList();
		
			// prologCodeNonEmptyList.Append(item);
		
			//Assert.AreEqual("expected", "result");
		}
		
		[Test]
		public void HeadTest ()
		{
            PrologCodePredicate head = new PrologCodePredicate("head");
            PrologCodeNonEmptyList prologCodeNonEmptyList = new PrologCodeNonEmptyList(head);

            object result = prologCodeNonEmptyList.Head;
		
			Assert.AreEqual(head, result);
		}
		
		[Test]
		public void TailTest ()
		{
            PrologCodeAtom head = new PrologCodeAtom();
            PrologCodeNonEmptyList tail = new PrologCodeNonEmptyList(head);
            PrologCodeNonEmptyList prologCodeNonEmptyList = new PrologCodeNonEmptyList(head, tail);

            PrologCodeTerm result = prologCodeNonEmptyList.Tail;
		
			Assert.AreEqual(tail, result);
		}
	}
}