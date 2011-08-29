//------------------------------------------------------------------------------
// <copyright file="PrologCodeProvider.cs" company="Axiom">
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
	public class PrologCodeProvider
	:	PrologCompilerModel
	{
		private PrologDotNetCodeGenerator generator = new PrologDotNetCodeGenerator ( );
		
		public override IPrologCodeGenerator CreateGenerator ()
		{
			return (IPrologCodeGenerator)generator;
		}
		
		public override IPrologCompiler CreateCompiler ()
		{
			return (IPrologCompiler)generator;
		}
	}
	
	internal class PrologDotNetCodeGenerator
	:	PrologCompiler
	{
		public PrologDotNetCodeGenerator ()
		{
		
		}
		
		/* This will be used to generate code from Prolog to another language. */
	}
}