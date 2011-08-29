//------------------------------------------------------------------------------
// <copyright file="PrologCodeConstantAtom.cs" company="Axiom">
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
	/// Represents a Prolog constant atom.
	/// </summary>
	public class PrologCodeConstantAtom
	:	PrologCodeAtom
	{
		private string _value;
		
		/// <summary>
		/// Initializes an instance of a PrologCodeConstantAtom class.
		/// </summary>
		/// <param name="value"></param>
		public PrologCodeConstantAtom (string value)
		{
			this._value = value;
		}
		
		/// <summary>
		/// Gets or sets the string value of the atom.
		/// </summary>
		public string Value
		{
			get
			{
				return this._value;
			}
			
			set
			{
				this._value = value;
			}
		}

        public override string ToString()
        {
            return _value;
        }
	}
}