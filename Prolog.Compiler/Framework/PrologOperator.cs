//------------------------------------------------------------------------------
// <copyright file="PrologOperator.cs" company="Axiom">
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
    /// <summary>
    /// Summary description for PrologOperator.
    /// </summary>
    public class PrologOperator
    {
        private string _name;
        private int _prefixPrecedence = 0;
        private int _postfixPrecedence = 0;
        private int _infixPrecedence = 0;
        // associativity
        private bool _infixLeftAssociative = false;
        private bool _infixRightAssociative = false;
        private bool _postfixAssociative = false;
        private bool _prefixAssociative = false;


        public PrologOperator(string name, int prefixPrecedence, int postfixPrecedence, int infixPrecedence)
        {
            _name = name;
            _infixLeftAssociative = false;
            _infixRightAssociative = false;
            _prefixAssociative = false;
            _postfixAssociative = false;

            _prefixPrecedence = prefixPrecedence;
            _postfixPrecedence = postfixPrecedence;
            _infixPrecedence = infixPrecedence;
        }

        public int InfixLeftPrecedence
        {
            get
            {
                return _infixPrecedence - 1 + ((_infixLeftAssociative) ? 1 : 0);
            }
        }

        public int InfixRightPrecedence
        {
            get
            {
                return _infixPrecedence - 1 + ((_infixRightAssociative) ? 1 : 0);
            }
        }

        public int PostfixLeftPrecedence
        {
            get
            {
                return _postfixPrecedence - 1 + ((_postfixAssociative) ? 1 : 0);
            }
        }

        public int PrefixRightPrecedence
        {
            get
            {
                return _prefixPrecedence - 1 + ((_prefixAssociative) ? 1 : 0);
            }
        }

        public int PrefixPrecedence
        {
            get { return _prefixPrecedence; }
            set { _prefixPrecedence = value; }
        }

        public int InfixPrecedence
        {
            get { return _infixPrecedence; }
            set { _infixPrecedence = value; }
        }

        public int PostfixPrecedence
        {
            get { return _postfixPrecedence; }
            set { _postfixPrecedence = value; }
        }

        public bool IsInfix
        {
            get { return _infixPrecedence != 0; }
        }

        public bool IsPrefix
        {
            get { return _prefixPrecedence != 0; }
        }

        public bool IsExclusivelyPrefix
        {
            get { return (_infixPrecedence == 0) && (_postfixPrecedence == 0); }
        }

        public bool IsPostfix
        {
            get { return _postfixPrecedence != 0; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsInfixLeftAssociative
        {
            get { return _infixLeftAssociative; }
            set { _infixLeftAssociative = value; }
        }

        public bool IsInfixRightAssociative
        {
            get { return _infixRightAssociative; }
            set { _infixRightAssociative = value; }
        }

        public bool IsPostfixAssociative
        {
            get { return _postfixAssociative; }
            set { _postfixAssociative = value; }
        }

        public bool IsPrefixAssociative
        {
            get { return _prefixAssociative; }
            set { _prefixAssociative = value; }
        }


    }
}