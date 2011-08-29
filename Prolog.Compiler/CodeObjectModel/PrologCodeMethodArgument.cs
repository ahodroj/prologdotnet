//------------------------------------------------------------------------------
// <copyright file="PrologCodeMethodArgument.cs" company="Axiom">
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
	/// Represents a foreign imported method argument.
	/// </summary>
	public class PrologCodeMethodArgument
	:	PrologCodeTerm
	{
		private int _type;
		private int _passing;

        // Passing types
        public const int PASS_IN = 0;
        public const int PASS_OUT = 1;
        public const int PASS_INOUT = 2;

        // data types
        public const int STRING = 0;
        public const int CHAR = 1;
        public const int INT = 2;
        public const int FLOAT = 3;
        public const int TERM = 4;
        public const int BOOL = 5;

		
		/// <summary>
		/// Iniitalizes a new instance of the PrologCodeMethodArgument class.
		/// </summary>
		/// <param name="_type"></param>
		/// <param name="_passing"></param>
		public PrologCodeMethodArgument (int _type, int _passing)
		{
			this._type = _type;
			this._passing = _passing;
		}
		
		/// <summary>
		/// Gets or sets the type of the method argument.
		/// </summary>
		public int Type
		{
			get
			{
				return this._type;
			}
			
			set
			{
				this._type = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the passing type of the method argument.
		/// </summary>
		public int Passing
		{
			get
			{
				return this._passing;
			}
			
			set
			{
				this._passing = value;
			}
		}
	}
}