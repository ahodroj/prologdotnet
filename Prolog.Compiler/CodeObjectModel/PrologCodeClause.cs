//------------------------------------------------------------------------------
// <copyright file="PrologCodeClause.cs" company="Axiom">
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
using System.Collections;

namespace Axiom.Compiler.CodeObjectModel
{	
	/// <summary>
	/// Represents a Prolog clause term.
	/// </summary>
	public class PrologCodeClause
	:	PrologCodeTerm
	{
		private ArrayList _goals;
		private PrologCodePredicate _head;
		
		/// <summary>
		/// Initializes a new instance of PrologCodeClause class.
		/// </summary>
		/// <param name="_head">clause head predicate.</param>
		public PrologCodeClause (PrologCodePredicate _head)
		{
			this._head = _head;
			this._goals = new ArrayList();
		}
		
		/// <summary>
		/// Initializes a new instance of PrologCodeClause class.
		/// </summary>
		public PrologCodeClause ()
		{
			this._head = null;
			this._goals = new ArrayList();
		}
		
		/// <summary>
		/// Gets the clause arity.
		/// </summary>
		public int Arity
		{
			get
			{
                if (_head == null)
                {
                    return 0;
                }
				return this._head.Arity;
			}
		}
		
		/// <summary>
		/// Gets the collection of clause goals.
		/// </summary>
		public ArrayList Goals
		{
			get
			{
				return this._goals;
			}
		}
		
		/// <summary>
		/// Gets or sets the clause head predicate.
		/// </summary>
		public PrologCodePredicate Head
		{
			get
			{
				return this._head;
			}
			
			set
			{
				this._head = value;
			}
		}

        public override string ToString()
        {
            string clauseStr = _head.ToString() + " :- ";
            for (int i = 0; i < _goals.Count; i++)
            {
                PrologCodeTerm term = (PrologCodeTerm)_goals[i];
                clauseStr += term.ToString();
                if (i == _goals.Count - 1)
                {
                    clauseStr += " .";
                }
                else
                {
                    clauseStr += ", ";
                }
            }
            return clauseStr;
        }
    }
}