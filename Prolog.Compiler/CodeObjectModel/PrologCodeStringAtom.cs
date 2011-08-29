//------------------------------------------------------------------------------
// <copyright file="PrologCodeStringAtom.cs" company="Axiom">
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
	/// Represents a Prolog string atom.
	/// </summary>
	public class PrologCodeStringAtom
	:	PrologCodeAtom
	{
		private string _value;
		
		public PrologCodeStringAtom (string _value)
		{
			this._value = _value;
		}
		
		public string Value
		{
			get
			{
				return this._value;
			}
		}

        public override string ToString()
        {
            return _value;
        }
	}
}