//------------------------------------------------------------------------------
// <copyright file="PrologCodeFact.cs" company="Axiom">
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
using System.Collections;

namespace Axiom.Compiler.CodeObjectModel
{	
	public class PrologCodeFact
	:	PrologCodeTerm
	{
		private ArrayList _arguments;
		private string _name;
		
		public PrologCodeFact ()
		{
			this._arguments = null;
			this._name = null;
		}
		
		public PrologCodeFact (string name)
		{
			this._name = name;
			this._arguments = new ArrayList();
		}
		
		public ArrayList Arguments
		{
			get
			{
				return this._arguments;
			}
			
			set
			{
				this._arguments = value;
			}
		}
		
		public int Arity
		{
			get
			{
				return this._arguments.Count;
			}
		}
		
		public string Name
		{
			get
			{
				return this._name;
			}
		}
	}
}