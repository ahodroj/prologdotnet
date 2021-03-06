//------------------------------------------------------------------------------
// <copyright file="IPrologParser.cs" company="Axiom">
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
using Axiom.Compiler.CodeObjectModel;

namespace Axiom.Compiler.Framework
{	
	public interface IPrologParser
	{
		PrologCodeUnit Parse (TextReader stream);
	}
}