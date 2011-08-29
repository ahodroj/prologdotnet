//------------------------------------------------------------------------------
// <copyright file="IPrologCodeGenerator.cs" company="Axiom">
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
using Axiom.Compiler.CodeObjectModel;
using System.Collections;

namespace Axiom.Compiler.Framework
{	
	public interface IPrologCodeGenerator
	{
		void GenerateCodeFromUnit (PrologCodeUnit u, ArrayList a);
		void GenerateCodeFromClause (PrologCodeClause c, ArrayList a);
		void GenerateCodeFromPredicate (PrologCodePredicate p, ArrayList a);
	}
}