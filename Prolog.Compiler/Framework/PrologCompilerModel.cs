//------------------------------------------------------------------------------
// <copyright file="PrologCompilerModel.cs" company="Axiom">
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
using System.IO;

namespace Axiom.Compiler.Framework
{	
	public abstract class PrologCompilerModel
	{
		public abstract IPrologCodeGenerator CreateGenerator ();
		
		public virtual IPrologCodeGenerator CreateGenerator (TextWriter output)
		{
			return CreateGenerator();
		}
		
		public virtual IPrologCodeGenerator CreateGenerator (string fileName)
		{
			return CreateGenerator();
		}
		
		public abstract IPrologCompiler CreateCompiler ();
		
		public virtual IPrologParser CreateParser ()
		{
			return null;
		}
	}
}