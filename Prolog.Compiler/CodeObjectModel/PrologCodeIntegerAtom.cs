//------------------------------------------------------------------------------
// <copyright file="PrologCodeIntegerAtom.cs" company="Axiom">
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
	/// Represents a Prolog integer atom.
	/// </summary>
	public class PrologCodeIntegerAtom
	:	PrologCodeAtom
	{
		private int _value;
		
		/// <summary>
		/// Initializes a new instance of the PrologCodeIntegerAtom class.
		/// </summary>
		public PrologCodeIntegerAtom ()
		{
			this._value = 0;
		}
		
		/// <summary>
		/// Initializes a new instance of the PrologCodeIntegerAtom class.
		/// </summary>
		/// <param name="_value"></param>
		public PrologCodeIntegerAtom (int _value)
		{
			this._value = _value;
		}
		
		/// <summary>
		/// Gets or sets the integer value of the atom.
		/// </summary>
		public int Value
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
            return _value.ToString();
        }
	}
}