//------------------------------------------------------------------------------
// <copyright file="PrologCompilerParameters.cs" company="Axiom">
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
	public class PrologCompilerParameters
	{
		private string _mainClass;
		private string _outputAssembly;
		private bool _generateExecutable = false;
		
		public PrologCompilerParameters ()
		{
			this._mainClass = "";
			this._outputAssembly = "";
			this._generateExecutable = false;
		}
		
		public string MainClass
		{
			get
			{
				return this._mainClass;
			}
			
			set
			{
				this._mainClass = value;
			}
		}
		
		public string OutputAssembly
		{
			get
			{
				return this._outputAssembly;
			}
			
			set
			{
				this._outputAssembly = value;
			}
		}
		
		public bool GenerateExecutable
		{
			get
			{
				return this._generateExecutable;
			}
			
			set
			{
				this._generateExecutable = value;
			}
		}
	}
}