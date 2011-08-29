//------------------------------------------------------------------------------
// <copyright file="PrologCodeList.cs" company="Axiom">
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
	/// Provides a common base for empty and non-empty Prolog lists.
	/// </summary>
	public class PrologCodeList
	:	PrologCodeTerm
	{
		/// <summary>
		/// Appends an item to the list.
		/// </summary>
		/// <param name="item">item to append.</param>
		public virtual void Append (PrologCodeTerm item)
		{
			
		}
	}
}