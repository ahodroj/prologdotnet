//------------------------------------------------------------------------------
// <copyright file="PrologCodeTerm.cs" company="Axiom">
//     
//      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
//     
//      The use and distribution terms for this source code are contained in the file
//      named license.txt, which can be found in the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by the
//      terms of this license.
//     
//      You must not remove this notice, or any other, from this software.
//     
// </copyright>                                                                
//------------------------------------------------------------------------------



using System;

namespace Axiom.Compiler.CodeObjectModel
{	
	/// <summary>
	/// Provides a base class for all Prolog Code Object Model objects.
	/// </summary>
	public abstract class PrologCodeTerm
	{
		public static bool IsAtom(PrologCodeTerm term) 
		{
			if(term is PrologCodeAtom) 
			{
				return true;
			}
			return false;
		}

		public static bool IsVariable(PrologCodeTerm term) 
		{
			if(term is PrologCodeVariable) 
			{
				return true;
			}
			return false;
		}

		public static bool IsList(PrologCodeTerm term) 
		{
			if(term is PrologCodeNonEmptyList || term is PrologCodeEmptyList) 
			{
				return true;
			}
			return false;
		}

		public static bool IsStruct(PrologCodeTerm term) 
		{
			if(term is PrologCodeClause) 
			{
				return ((PrologCodeClause)term).Arity > 0;
			}
			if(term is PrologCodeFact) 
			{
				return ((PrologCodeFact)term).Arity > 0;
			}
			if(term is PrologCodePredicate) 
			{
				return ((PrologCodePredicate)term).Arity > 0;
			}
			return false;
		}

		public static bool IsAtomicPredicate(PrologCodeTerm term) 
		{
			if(term is PrologCodeClause) 
			{
				return ((PrologCodeClause)term).Arity == 0;
			}
			if(term is PrologCodeFact) 
			{
				return ((PrologCodeFact)term).Arity == 0;
			}
			if(term is PrologCodePredicate) 
			{
				return ((PrologCodePredicate)term).Arity == 0;
			}
			return false;
		}
		
	}
}