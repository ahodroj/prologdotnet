//------------------------------------------------------------------------------
// <copyright file="PrologToken.cs" company="Axiom">
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


namespace Axiom.Compiler.Framework
{	
	using System;
	using System.Text;

	public class PrologToken
	{
		/// <summary>
		/// Token codes.
		/// </summary>
		public const int
			NONE			= 0,	// Error Token
			EOF				= 1,	// End of file
			STRING			= 2,	// string
			ATOM			= 3,	// atom
			VARIABLE		= 4,	// variable
			LPAREN			= 5,	// (
			RPAREN			= 6,	// )
			DOT				= 7,	// .
			LBRACKET		= 8,	// [
			RBRACKET		= 9,	// ]
			LIST_SEP		= 10,	// |
			COMMA			= 11;	// ,
           
		
		/// <summary>
		/// Token names.
		/// </summary>
		public static readonly string[] names = {
													"?",
													"End of File", 
													"String", 
													"Atom",
													"Variable",
													"Left Paren.",
													"Right Paren.",
													"Dot",
													"Left Bracket",	
													"Right Bracket",
													"List Sep.",
													"Comma",
                                                   
												};	
		private int _kind;    // token code (NONE, IDENT, ...)
		public int Kind 
		{
			get { return _kind; }
			set { _kind = value; }
		}


		private int _line;    // token line number (for error messages)
		public int Line 
		{
			get { return _line; }
			set { _line = value; }
		}


		private int _col;     // token column number (for error messages)
		public int Column 
		{
			get { return _col; }
			set { _col = value; }
		}


		private int _intValue;     // numerical value (for numbers and character constants)
		public int IntValue 
		{
			get { return _intValue; }
			set { _intValue = value; }
		}

		private string _stringValue;  // string representation of token (for numbers and identifiers)
		/* We keep the string representation of numbers for error messages in case the
		 * number literal in the source code is too big to fit into an int.
		 */
		public string StringValue 
		{
			get { return _stringValue; }
			set { _stringValue = value; }
		}

		public PrologToken (int line, int col) : this(NONE, line, col, 0, null) {}

		public PrologToken (int kind, int line, int col) : this(kind, line, col, 0, null) {}

		public PrologToken (int kind, int line, int col, int val) : this(kind, line, col, val, null) {}
		
		public PrologToken (int kind, int line, int col, string str) : this(kind, line, col, 0, str) {}
		
		public PrologToken (int kind, int line, int col, int val, string str) 
		{
			this._kind = kind;
			this._line = line; 
			this._col = col;
			this._intValue = val; 
			this._stringValue = str;
		}

		public override string ToString () 
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("line {0}, col {1}: {2}", _line, _col, names[_kind]);
			sb.AppendFormat("	Name: {0}", _stringValue);
			return sb.ToString();
		}	
		
	
	}
}