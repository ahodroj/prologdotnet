//------------------------------------------------------------------------------
// <copyright file="PrologCodePredicate.cs" company="Axiom">
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
	/// Represents a Prolog predicate.
	/// </summary>
	public class PrologCodePredicate
	:	PrologCodeTerm
	{
		private ArrayList _arguments;
		private string _name;
        bool _isMethod = false;
        bool _isBuiltIn = false;
        private PrologCodeMethod _methodInfo = null;
		
		public PrologCodePredicate ()
		{
			this._arguments = new ArrayList();
			this._name = null;
		}
		
		public PrologCodePredicate (string name)
		{
			this._name = name;
			this._arguments = new ArrayList();
		}

        public PrologCodeMethod MethodInfo
        {
            get { return _methodInfo; }
            set { _methodInfo = value; }
        }

		public ArrayList Arguments
		{
			get
			{
				return this._arguments;
			}
			
			set
			{
				this._arguments = value;
			}
		}
		
		public int Arity
		{
			get
			{
				return this._arguments.Count;
			}
		}
		
		public string Name
		{
			get
			{
				return this._name;
			}
		}

        public bool IsMethod
        {
            get { return _isMethod; }
            set { _isMethod = value; }
        }

        public bool IsBuiltIn
        {
            get { return _isBuiltIn; }
            set { _isBuiltIn = value; }
        }
        public override string ToString()
        {
            string predicateStr = _name;
            // For testing purposes, this can be fixed by implementing WriteTerm
            if (_name == "=")
            {
                predicateStr = ((PrologCodeTerm)_arguments[0]).ToString() + " = " + ((PrologCodeTerm)_arguments[1]).ToString();
 
            }
            else
            {
                if (_arguments.Count != 0)
                {
                    predicateStr += "(";
                    for (int i = 0; i < _arguments.Count; i++)
                    {
                        PrologCodeTerm term = (PrologCodeTerm)_arguments[i];
                        predicateStr += term.ToString();
                        if (i != _arguments.Count - 1)
                        {
                            predicateStr += ",";
                        }
                    }
                    predicateStr += ")";
                }
            }

            return predicateStr;
        }
	}
}