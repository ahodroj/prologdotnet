//------------------------------------------------------------------------------
// <copyright file="PrologOperatorTable.cs" company="Axiom">
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
using System.IO;
using Axiom.Compiler.CodeObjectModel;

namespace Axiom.Compiler.Framework
{
    /// <summary>
    /// Summary description for PrologCodeParser.
    /// </summary>
    public class PrologOperatorTable
    {
        private Hashtable _operatorTable = new Hashtable();

        public PrologOperatorTable()
        {
        }

        public void Initialize()
        {
            /* xfx */
            AddInfixOperator(":-", false, false, 1200);
            AddInfixOperator("-->", false, false, 1200);
            AddInfixOperator("<=", false, false, 1190);
            AddInfixOperator("<->", false, false, 1190);
            AddInfixOperator("<-", false, false, 1190);
            AddInfixOperator("until", false, false, 990);
            AddInfixOperator("unless", false, false, 990);
            AddInfixOperator("from", false, false, 800);
            AddInfixOperator("=:=", false, false, 700);
            AddInfixOperator("=\\=", false, false, 700);
            AddInfixOperator("<", false, false, 700);
            AddInfixOperator(">=", false, false, 700);
            AddInfixOperator(">", false, false, 700);
            AddInfixOperator("=<", false, false, 700);
            AddInfixOperator("is", false, false, 700);
            AddInfixOperator("=..", false, false, 700);
            AddInfixOperator("==", false, false, 700);
            AddInfixOperator("\\==", false, false, 700);
            AddInfixOperator("=", false, false, 700);
            AddInfixOperator("\\=", false, false, 700);
            AddInfixOperator("@<", false, false, 700);
            AddInfixOperator("@>=", false, false, 700);
            AddInfixOperator("@>", false, false, 700);
            AddInfixOperator("@=<", false, false, 700);
            AddInfixOperator("mod", false, false, 300);
            AddInfixOperator(":", false, false, 300);

            /* xfy */
            AddInfixOperator(",", false, true, 1000);
            AddInfixOperator(";", false, true, 1100);
            AddInfixOperator("->", false, true, 1050);
            AddInfixOperator(".", false, true, 999);
            AddInfixOperator(">>", false, true, 400);
            AddInfixOperator("^", false, true, 200);

            /* yfx */
            AddInfixOperator("+", true, false, 500);
            AddInfixOperator("-", true, false, 500);
            AddInfixOperator("\\/", true, false, 500);
            AddInfixOperator("/\\", true, false, 500);
            AddInfixOperator("*", true, false, 400);
            AddInfixOperator("/", true, false, 400);
            AddInfixOperator("div", true, false, 400);
            AddInfixOperator("//", true, false, 400);
            AddInfixOperator("<<", true, false, 400);

            /* fx */
            AddPrefixOperator(":-", false, 1200);
            AddPrefixOperator("?-", false, 1200);
            AddPrefixOperator("gen", false, 990);
            AddPrefixOperator("try", false, 980);
            AddPrefixOperator("once", false, 970);
            AddPrefixOperator("possible", false, 970);
            AddPrefixOperator("side_effects", false, 970);
            AddPrefixOperator("unit", false, 900);
            AddPrefixOperator("visible", false, 900);
            AddPrefixOperator("import", false, 900);
            AddPrefixOperator("foreign", false, 900);
            AddPrefixOperator("using", false, 900);
            AddPrefixOperator("push", false, 900);
            AddPrefixOperator("down", false, 900);
            AddPrefixOperator("set", false, 900);
            AddPrefixOperator("dynamic", false, 900);
            AddPrefixOperator("+", false, 500);
            AddPrefixOperator("-", false, 500);
            AddPrefixOperator("\\", false, 500);
            AddPrefixOperator("@", false, 10);
            AddPrefixOperator("@@", false, 10);

            /* fy */
            AddPrefixOperator("not", true, 980);
            AddPrefixOperator("\\+", true, 980);
            AddPrefixOperator("spy", true, 900);
            AddPrefixOperator("nospy", true, 900);
            AddPrefixOperator("?", true, 800);
            AddPrefixOperator(">", true, 700);
            AddPrefixOperator("<", true, 700);

            /* xf */
            //AddPostfixOp("!",		false,	999) ;
            AddPostfixOperator("#", false, 999);
		
        }

        public void RemoveOperator(string name)
        {
            if (_operatorTable.ContainsKey(name))
            {
                _operatorTable.Remove(name);
            }
        }

        public void AddInfixOperator(string name, bool left, bool right, int precedence)
        {
            PrologOperator op = null;
            if (IsOperator(name))
            {
                op = GetOperator(name);
                op.InfixPrecedence = precedence;
                op.IsInfixLeftAssociative = left;
                op.IsInfixRightAssociative = right;
            }
            else
            {
                op = new PrologOperator(name, 0, 0, precedence);
                op.IsInfixLeftAssociative = left;
                op.IsInfixRightAssociative = right;
                _operatorTable.Add(name, op);
            }

        }

        public void AddPrefixOperator(string name, bool left, int precedence)
        {
            PrologOperator op = null;
            if (IsOperator(name))
            {
                op = GetOperator(name);
                op.PrefixPrecedence = precedence;
                op.IsPrefixAssociative = left;
            }
            else
            {
                op = new PrologOperator(name, precedence, 0, 0);
                op.IsPrefixAssociative = left;
                _operatorTable.Add(name, op);
            }
        }

        public void AddPostfixOperator(string name, bool right, int precedence)
        {
            PrologOperator op = null;

            if (IsOperator(name))
            {
                op = GetOperator(name);
                op.PostfixPrecedence = precedence;
                op.IsPostfixAssociative = right;
            }
            else
            {
                op = new PrologOperator(name, 0, precedence, 0);
                op.IsPrefixAssociative = right;
                _operatorTable.Add(name, op);
            }
        }


        public bool IsOperator(string name)
        {
            return _operatorTable.ContainsKey(name);
        }

        public PrologOperator GetOperator(string name)
        {
            return (PrologOperator)_operatorTable[name];
        }

        public bool ExclusivelyPrefix(string name)
        {
            if (IsOperator(name))
            {
                return true;
            }
            PrologOperator op = GetOperator(name);
            if (op != null)
            {
                return op.PostfixPrecedence == -1;
            }
            return false;
        }
    }
}