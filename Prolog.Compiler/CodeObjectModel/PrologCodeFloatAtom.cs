//------------------------------------------------------------------------------
// <copyright file="PrologCodeFloatAtom.cs" company="Axiom">
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



namespace Axiom.Compiler.CodeObjectModel
{
    using System;

	/// <summary>
	/// Represents a Prolog floating number atom.
	/// </summary>
	public class PrologCodeFloatAtom
	:	PrologCodeAtom
	{
		private float _value;
		
		/// <summary>
		/// Initializes a new instance of the PrologCodeFloatAtom class.
		/// </summary>
		public PrologCodeFloatAtom ()
		{
			this._value = 0;
		}
		
		/// <summary>
		/// Initializes a new instance of the PrologCodeFloatAtom class.
		/// </summary>
		/// <param name="_value">float value of the atom.</param>
		public PrologCodeFloatAtom (float _value)
		{
			this._value = _value;
		}
		
		/// <summary>
		/// Gets or sets the float value of the atom.
		/// </summary>
		public float Value
		{
			get { return this._value; }
			set { this._value = value; }
		}

        public override string ToString()
        {
            return _value.ToString();
        }
	}
}