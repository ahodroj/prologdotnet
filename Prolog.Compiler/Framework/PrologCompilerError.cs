//------------------------------------------------------------------------------
// <copyright file="PrologCompilerError.cs" company="Axiom">
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

namespace Axiom.Compiler.Framework
{	
	public class PrologCompilerError //: System.Exception
	{
		private int _column;
		private int _line;
		private string _errorCode;
		private string _errorText;
		private string _fileName;
		private bool _isWarning;
		
		public PrologCompilerError (string errorCode, string errorText, string fileName, bool isWarning, int line, int column)
		{
			this._column = column;
			this._line = line;
			this._errorCode = errorCode;
			this._errorText = errorText;
			this._fileName = fileName;
			this._isWarning = isWarning;
		}

        public override string ToString()
        {
            string errorStr = "Error " + _errorCode + ": " + _errorText + " (" + _fileName + ")";
            return errorStr;
        }

        
		
		public int Column
		{
			get
			{
				return this._column;
			}
			
			set
			{
				this._column = value;
			}
		}
		
		public int Line
		{
			get
			{
				return this._line;
			}
			
			set
			{
				this._line = value;
			}
		}
		
		public string ErrorCode
		{
			get
			{
				return this._errorCode;
			}
			
			set
			{
				this._errorCode = value;
			}
		}
		
		public string ErrorText
		{
			get
			{
				return this._errorText;
			}
			
			set
			{
				this._errorText = value;
			}
		}
		
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			
			set
			{
				this._fileName = value;
			}
		}
		
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			
			set
			{
				this._isWarning = value;
			}
		}
	}
}