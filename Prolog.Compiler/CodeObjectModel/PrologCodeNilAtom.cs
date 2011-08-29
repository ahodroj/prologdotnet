//------------------------------------------------------------------------------
// <copyright file="PrologNilAtom.cs" company="Axiom">
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
    /// Provide a common base for different types for Prolog atoms.
    /// </summary>
    public class PrologCodeNilAtom
    : PrologCodeTerm
    {
        public override string ToString()
        {
            return "_";
        }
    }
}