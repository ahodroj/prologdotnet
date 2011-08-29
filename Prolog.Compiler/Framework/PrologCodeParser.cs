//------------------------------------------------------------------------------
// <copyright file="PrologCodeParser.cs" company="Axiom">
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
	public class PrologCodeParser : IPrologParser
	{
        private PrologScanner _scanner = null;
        private PrologOperatorTable _operators = new PrologOperatorTable();
        private PrologCodeUnit _codeUnit = new PrologCodeUnit();
        private ArrayList _errors = new ArrayList();
        private int _randomVariableID = 100;

        public PrologCodeParser()
        {
            _operators.Initialize();
        }

        public PrologCodeUnit Parse(TextReader input)
        {
            _scanner = new PrologScanner(input);
            _errors.Clear();

            PrologCodeTerm term = null;
            while(true)
            {

                term = ReadTerm(1200);
                if (_scanner.Next().Kind != PrologToken.DOT)
                {
                    _errors.Add(new PrologCompilerError("P0001", "Unexpected end of term", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    break;
                }
                if (term is PrologCodeHeadlessClause)
                {
                    ProcessHeadlessClause(term);
                }
                else
                {
                    _codeUnit.Terms.Add(term);
                }
                if (_scanner.Lookahead.Kind == PrologToken.EOF)
                    break;

            }
            return _codeUnit;
        }

        private void ProcessHeadlessClause(PrologCodeTerm term)
        {
            PrologCodeHeadlessClause clause = (PrologCodeHeadlessClause)term;
            foreach (PrologCodeTerm t in clause.Goals)
            {
                if (t is PrologCodePredicate)
                {
                    PrologCodePredicate pred = (PrologCodePredicate)t;
                    switch (pred.Name)
                    {
                        case "op":
                            ProcessOperator(pred);
                            break;
                        case "class":
                            PrologCodeTerm arg = (PrologCodeTerm)pred.Arguments[0];
                            if (arg is PrologCodeConstantAtom)
                            {
                                PrologCodeConstantAtom atom = (PrologCodeConstantAtom)arg;
                                _codeUnit.Class = atom.Value;
                            }
                            else if (arg is PrologCodeStringAtom)
                            {
                                PrologCodeStringAtom atom = (PrologCodeStringAtom)arg;
                                _codeUnit.Class = atom.Value.Replace("'", "");
                            }
                            else
                            {
                                _errors.Add(new PrologCompilerError("P0002", "Illegal class name.", "", false, _scanner.Current.Line, _scanner.Current.Column));

                            }
                            break;
                        case "foreign":
                            ProcessForeignMethod(pred);
                            break;
                        case "load_assembly":
                            ProcessAssemblyDirective(pred);
                            break;
                        case "using":
                            ProcessUsingDirective(pred);
                            break;
                    }
                }
                else if (t is PrologCodeList)
                {
                }
            }
        }

        private void ProcessUsingDirective(PrologCodePredicate pred)
        {
            string ns = ((PrologCodeStringAtom)pred.Arguments[0]).Value;
            ns = ns.Replace("'", "");
            _codeUnit.Namespaces.Add(ns);
        }

        private void ProcessAssemblyDirective(PrologCodePredicate pred)
        {
            string asm = ((PrologCodeStringAtom)pred.Arguments[0]).Value;
            asm = asm.Replace("'", "");
            _codeUnit.AssemblyFiles.Add(asm);
        }

        private void ProcessOperator(PrologCodePredicate p)
        {
            if (!(p.Arguments[0] is PrologCodeIntegerAtom))
            {
                _errors.Add(new PrologCompilerError("P0009", "Invalid operator priority.", "", false, _scanner.Current.Line, _scanner.Current.Column));
            }

            int priority = ((PrologCodeIntegerAtom)p.Arguments[0]).Value;

            if (!(p.Arguments[1] is PrologCodeConstantAtom))
            {
                _errors.Add(new PrologCompilerError("P0010", "Invalid operator associativity specifier.", "", false, _scanner.Current.Line, _scanner.Current.Column));
            }

            string associativity = ((PrologCodeConstantAtom)p.Arguments[1]).Value;

            if (p.Arguments[2] is PrologCodeNonEmptyList)
            {
                ArrayList operators = GetListOperators((PrologCodeNonEmptyList)p.Arguments[2]);

                foreach (PrologCodeTerm op in operators)
                {
                    DefineNewOperator(priority, associativity, op);
                }

            }
            else if(p.Arguments[2] is PrologCodeConstantAtom)
            {

                DefineNewOperator(priority, associativity, (PrologCodeTerm)p.Arguments[2]);
            }
            else if (p.Arguments[2] is PrologCodeStringAtom)
            {
                DefineNewOperator(priority, associativity, (PrologCodeTerm)p.Arguments[2]);
            }
            else
            {
                _errors.Add(new PrologCompilerError("P0011", "Invalid operator definition.", "", false, _scanner.Current.Line, _scanner.Current.Column));
            }
        }

        private ArrayList GetListOperators(PrologCodeNonEmptyList list)
        {
            ArrayList listMembers = new ArrayList();

            for (PrologCodeTerm l = list.Head; !(l is PrologCodeEmptyList); l = list.Tail)
            {
                if (l is PrologCodeAtom)
                {
                    listMembers.Add(l);
                }
                else
                {
                   _errors.Add(new PrologCompilerError("P0011", "Invalid operator definition.", "", false, _scanner.Current.Line, _scanner.Current.Column));
                }
            }
            return listMembers;
        }

        private void DefineNewOperator(int pri, string assoc, PrologCodeTerm op)
        {
            // Check operator name
            string opValue = "";
            if (op is PrologCodeConstantAtom)
            {
                opValue = ((PrologCodeConstantAtom)op).Value;
            }
            else if (op is PrologCodeStringAtom)
            {
                opValue = ((PrologCodeStringAtom)op).Value;
            }
            else
            {
                _errors.Add(new PrologCompilerError("P0011", "Invalid operator definition.", "", false, _scanner.Current.Line, _scanner.Current.Column));
                return;
            }

            if (opValue == "," || opValue == "','")
            {
                _errors.Add(new PrologCompilerError("P0011", "Invalid operator definition.", "", false, _scanner.Current.Line, _scanner.Current.Column));
                return;
            }
            // Check operator priority
            if (pri > 0 && pri < 1200)
            {
                UpdateOperatorTable(pri, assoc, opValue);
            }
            else if (pri == 0)
            {
                // Remove an operator
                _operators.RemoveOperator(opValue);
            }
            else
            {
                // Error
            }
        }

        private void UpdateOperatorTable(int priority, string associativity, string name)
        {
            switch (associativity)
            {
                case "xfx":
                    /* xfx */
                    _operators.AddInfixOperator(name, false, false, priority);
                    break;
                case "xfy":
                    _operators.AddInfixOperator(name, false, true, priority);
                    break;
                case "yfx":
                    _operators.AddInfixOperator(name, true, false, priority);
                    break;
                case "fx":
                    _operators.AddPrefixOperator(name, false, priority);
                    break;
                case "fy":
                    _operators.AddPrefixOperator(name, true, priority);
                    break;
                case "xf":
                    _operators.AddPostfixOperator(name, false, priority);
                    break;
                default:
                    _errors.Add(new PrologCompilerError("P0010", "Invalid operator associativity specifier.", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    break;
            }
        }

        private void ProcessForeignMethod(PrologCodePredicate p)
        {
            // :- foreign(functor(+term,...),'Assembly','Class','MethodName').
            PrologCodeMethod foreignMethod = new PrologCodeMethod();
            PrologCodePredicate predicateFunctor = (PrologCodePredicate)p.Arguments[0];

            // Add argument types
            foreignMethod.Arguments = GetForeignMethodArguments(predicateFunctor);
            foreignMethod.AssemblyName = GetAtomOrStringValue((PrologCodeTerm)p.Arguments[1]);
            foreignMethod.Class = GetAtomOrStringValue((PrologCodeTerm)p.Arguments[2]);
            foreignMethod.PredicateName = predicateFunctor.Name;
            foreignMethod.MethodName = predicateFunctor.Name.Replace("'", "");

            if (p.Arguments.Count == 4)
            {
                foreignMethod.MethodName = GetAtomOrStringValue((PrologCodeTerm)p.Arguments[3]);
            }
            
            // Add the method
            _codeUnit.Methods.Add(foreignMethod);
            
        }

        private string GetAtomOrStringValue(PrologCodeTerm term)
        {
            if (term is PrologCodeConstantAtom)
            {
                return ((PrologCodeConstantAtom)term).Value;
            }
            else if (term is PrologCodeStringAtom)
            {
                return ((PrologCodeStringAtom)term).Value.Replace("'", "");
            }
            return null;
        }

        private ArrayList GetForeignMethodArguments(PrologCodePredicate f)
        {
            ArrayList args = new ArrayList();
            if (f.Arguments[0] is PrologCodeConstantAtom)
            {
                PrologCodeConstantAtom fc = (PrologCodeConstantAtom)f.Arguments[0];
                if (fc.Value == "none")
                {
                    return args;
                }
                else
                {
                    _errors.Add(new PrologCompilerError("P0012", "Invalid predicate-method definition", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    return args;
                }
            }
            PrologCodePredicate functor = (PrologCodePredicate)f;
            foreach (PrologCodePredicate a in functor.Arguments)
            {
                int passing = 0;
                int datatype = 0;

                switch (a.Name)
                {
                    case "+":
                        passing = PrologCodeMethodArgument.PASS_IN;
                        break;
                    case "-":
                        passing = PrologCodeMethodArgument.PASS_OUT;
                        break;
                    case "?":
                        passing = PrologCodeMethodArgument.PASS_INOUT;
                        break;
                    default:
                        break;
                }
                switch (((PrologCodeConstantAtom)a.Arguments[0]).Value)
                {
                    case "string":
                        datatype = PrologCodeMethodArgument.STRING;
                        break;
                    case "char":
                        datatype = PrologCodeMethodArgument.CHAR;
                        break;
                    case "int":
                        datatype = PrologCodeMethodArgument.INT;
                        break;
                    case "float":
                        datatype = PrologCodeMethodArgument.FLOAT;
                        break;
                    case "term":
                        datatype = PrologCodeMethodArgument.TERM;
                        break;
                    case "bool":
                        datatype = PrologCodeMethodArgument.BOOL;
                        break;
                    default:
                        break;
                }
                args.Add(new PrologCodeMethodArgument(datatype, passing));
            }
            return args;
        }

        public PrologCodeTerm ReadTerm(int priority)
        {
            /* Read a term as binary tree */
            BinaryTree ast = Term(1200);

            /* Convert binary tree into PrologCodeTerm and return it*/
            return ConvertBinaryTreeToCodeDOM(ast);
        }

        public BinaryTree Term(int n)
        {
            int m = 0;
            BinaryTree ast = null; 
            _scanner.Next();
            switch (_scanner.Current.Kind)
            {
                case PrologToken.LPAREN:
                    ast = Term(1200);
                    _scanner.Next();
                    if (_scanner.Current.Kind != PrologToken.RPAREN)
                    {
                        _errors.Add(new PrologCompilerError("P0003", "Expected ) after term", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    }
                    break;
                // Handle list terms here...
                case PrologToken.LBRACKET:
                    ArrayList listArguments = new ArrayList();
                    // Peek after [ token
                    if (_scanner.Lookahead.Kind == PrologToken.RBRACKET)
                    {
                        _scanner.Next();
                        // return a nil atom: []
                        ast = new BinaryList(); // empty list
                        break;
                    }
                    listArguments.Add(Term(999));
                    _scanner.Next();

                    // if , is encountered
                    while (_scanner.Current.Kind == PrologToken.COMMA)
                    {
                        listArguments.Add(Term(999));
                        _scanner.Next();
                    }
                    if (_scanner.Current.Kind == PrologToken.LIST_SEP)
                    {
                        listArguments.Add(Term(999));
                        _scanner.Next();
                    }
                    else
                    {
                        listArguments.Add(new BinaryList());
                    }
                    if (_scanner.Current.Kind != PrologToken.RBRACKET)
                    {
                        _errors.Add(new PrologCompilerError("P0004", "Unterminated list, expected ]", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    }

                    int i = listArguments.Count - 1;
                    BinaryList list = new BinaryList((BinaryTree)listArguments[i - 1], (BinaryTree)listArguments[i]);
                    i -= 2;
                    while (i > -1)
                    {
                        list = new BinaryList((BinaryTree)listArguments[i--], list);
                    }
                    ast = list;
                    break;

                case PrologToken.RPAREN:
                case PrologToken.RBRACKET:
                case PrologToken.COMMA:
                case PrologToken.DOT:
                    _errors.Add(new PrologCompilerError("P0005", "Unexpected closure of term", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    return null;
                case PrologToken.ATOM:
                case PrologToken.VARIABLE:
                    string atomName = _scanner.Current.StringValue;
                    if (_scanner.Lookahead.Kind == PrologToken.LPAREN)
                    {
                        ArrayList arguments = new ArrayList();
                        _scanner.Next();
                        arguments.Add(Term(1200));

                        _scanner.Next();
                        while (_scanner.Current.Kind == PrologToken.COMMA)
                        {
                            arguments.Add(Term(1200));
                            _scanner.Next();
                        }
                        if (_scanner.Current.Kind != PrologToken.RPAREN)
                        {
                            _errors.Add(new PrologCompilerError("P0005", "Unexpected closure of term", "", false, _scanner.Current.Line, _scanner.Current.Column));
                            return null;
                        }
                        ast = new BinaryTree(atomName, arguments);
                        break;
                    }
                    if (_operators.IsOperator(_scanner.Current.StringValue))
                    {
                        PrologOperator op = _operators.GetOperator(_scanner.Current.StringValue);
                        if (op.IsPrefix)
                        {
                            // prefix operator
                            if (n < op.PrefixPrecedence)
                            {
                                ParserError("prefix precedence error", _scanner.Current.Line, _scanner.Current.Column);
                                return null;
                            }
                            switch (_scanner.Lookahead.Kind)
                            {
                                case PrologToken.LPAREN:
                                case PrologToken.LBRACKET:
                                    ast = new BinaryTree(op.Name, Term(op.PrefixRightPrecedence));
                                    m = op.PrefixPrecedence;
                                    Right(n, m, ref ast);
                                    break;
                                case PrologToken.RPAREN:
                                case PrologToken.RBRACKET:
                                case PrologToken.DOT:
                                case PrologToken.LIST_SEP:
                                    if (n < m)
                                    {
                                        _errors.Add(new PrologCompilerError("P0006", "Unexpected atom '" + _scanner.Lookahead.StringValue + "'", "", false, _scanner.Current.Line, _scanner.Current.Column));
                                        return null;
                                    }
                                    ast = new BinaryTree(_scanner.Current.StringValue);
                                    Right(n, m, ref ast);
                                    break;
                                case PrologToken.ATOM:
                                    if (_operators.IsOperator(_scanner.Lookahead.StringValue))
                                    {
                                        PrologOperator atomOp = _operators.GetOperator(_scanner.Lookahead.StringValue);
                                        if (atomOp.IsInfix && m <= atomOp.InfixLeftPrecedence)
                                        {
                                            if (n < m)
                                            {
                                                ParserError("n < m", _scanner.Lookahead.Line, _scanner.Current.Column);
                                                return null;
                                            }
                                            ast = new BinaryTree(_scanner.Lookahead.StringValue);
                                            Right(n, m, ref ast);
                                            break;
                                        }
                                        else if (atomOp.IsPostfix && m <= atomOp.PostfixLeftPrecedence)
                                        {
                                            if (n < m)
                                            {
                                                ParserError("n < m", _scanner.Current.Line, _scanner.Current.Column);
                                                return null;
                                            }
                                            ast = new BinaryTree(_scanner.Current.StringValue);
                                            Right(n, m, ref ast);
                                            break;
                                        }
                                        // Just added on 6/23/2006. Might not fix anything.
                                        else
                                        {
                                            ast = new BinaryTree(_scanner.Current.StringValue, null, Term(op.InfixRightPrecedence));
                                            m = op.PrefixPrecedence;
                                            Right(n, m, ref ast);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ast = new BinaryTree(_scanner.Current.StringValue, null, Term(op.InfixRightPrecedence));
                                        m = op.PrefixPrecedence;
                                        Right(n, m, ref ast);
                                    }
                                    break;
                                default:
                                    ParserError("Unknown internal error", _scanner.Current.Line, _scanner.Current.Column);
                                    return null;
                            }
                        }
                    }
                    else
                    {
                        ast = new BinaryTree(_scanner.Current.StringValue);
                    }
                    break;
                default:
                    ParserError("Unknown internal error", _scanner.Current.Line, _scanner.Current.Column);
                    return null;

            }
            Right(n, m, ref ast);
            return ast;
        }

        private BinaryTree Right(int n, int m, ref BinaryTree result)
        {
            switch (_scanner.Lookahead.Kind)
            {
                case PrologToken.DOT:
                case PrologToken.RPAREN:
                case PrologToken.RBRACKET:
                    return result;
                case PrologToken.LPAREN:
                case PrologToken.LBRACKET:
                    _errors.Add(new PrologCompilerError("P0007", "Unexpected open brackets or parenthsis", "", false, _scanner.Current.Line, _scanner.Current.Column));
                    return result;
                case PrologToken.COMMA:
                    if (n >= 1000 && m <= 1000)
                    {
                        m = 1000;
                        _scanner.Next();
                        result = new BinaryTree(",", result, Term(m));
                        if (n > m)
                        {
                            Right(n, m, ref result);
                        }
                    }
                    return result;
                case PrologToken.ATOM:
                    PrologOperator laOp = _operators.GetOperator(_scanner.Lookahead.StringValue);
                    if (laOp != null)
                    {
                        if (laOp.IsPostfix &&
                            n >= laOp.PostfixPrecedence &&
                            m <= laOp.PostfixLeftPrecedence)
                        {
                            _scanner.Next();
                            if (_operators.IsOperator(_scanner.Current.StringValue))
                            {
                                PrologOperator o = _operators.GetOperator(_scanner.Current.StringValue);
                                if (o.IsInfix &&
                                   n >= o.InfixPrecedence &&
                                   m <= o.InfixLeftPrecedence)
                                {
                                    switch (_scanner.Lookahead.Kind)
                                    {
                                        case PrologToken.LPAREN:
                                        case PrologToken.LBRACKET:
                                            result = new BinaryTree(o.Name, result, Term(o.InfixRightPrecedence));
                                            m = o.InfixPrecedence;
                                            Right(n, m, ref result);
                                            break;
                                        case PrologToken.COMMA:
                                        case PrologToken.RPAREN:
                                        case PrologToken.RBRACKET:
                                            result = new BinaryTree(_scanner.Current.StringValue, result);
                                            m = o.InfixPrecedence;
                                            Right(n, m, ref result);
                                            break;
                                        case PrologToken.ATOM:
                                            if (_operators.IsOperator(_scanner.Lookahead.StringValue))
                                            {
                                                if (_operators.ExclusivelyPrefix(_scanner.Lookahead.StringValue))
                                                {
                                                    result = new BinaryTree(_scanner.Lookahead.StringValue, result, Term(_operators.GetOperator(_scanner.Lookahead.StringValue).PrefixRightPrecedence));
                                                    m = o.InfixPrecedence;
                                                    Right(n, m, ref result);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                result = new BinaryTree(_scanner.Lookahead.StringValue, result, null);
                                                m = _operators.GetOperator(_scanner.Lookahead.StringValue).InfixPrecedence;
                                                Right(n, m, ref result);
                                                break;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    result = new BinaryTree(_scanner.Current.StringValue, result);
                                    m = _operators.GetOperator(_scanner.Current.StringValue).InfixPrecedence;
                                    Right(n, m, ref result);
                                }
                            }
                            break;
                        }
                        else if (laOp.IsInfix && n >= laOp.InfixPrecedence && m <= laOp.InfixLeftPrecedence)
                        {
                            _scanner.Next();
                            int p = _operators.GetOperator(_scanner.Current.StringValue).InfixPrecedence;
                            int t = _operators.GetOperator(_scanner.Current.StringValue).InfixRightPrecedence;
                            result = new BinaryTree(_scanner.Current.StringValue, result, Term(t));
                            m = p;
                            Right(n, m, ref result);
                            break;
                        }
                    }
                    else
                    {
                        return result;
                    }
                    break;
            }
            return result;
        }

        private void ParserError(string error, int line, int column)
        {
            Console.WriteLine("Parser Internal Error: " + error + ". Line: " + line + ", Column: " + column);
        }


        private PrologCodeTerm ConvertGoalVariableBinaryTreeToCodeDOM(BinaryTree var)
        {
            if (Char.IsUpper(var.Name[0]) || var.Name[0] == '_' || var.Name == "_")
            {
                if (var.Name == "_")
                {
                    _randomVariableID++;
                    return new PrologCodeVariable(var.Name + "%" + _randomVariableID.ToString());
                }
                return new PrologCodeVariable(var.Name);
            }
            else
            {
                // Not a functor, => atom | number | string
                if (var.Arguments == null || var.Arguments.Count == 0)
                {
                    if (var.Name == ".")
                    {
                        // TODO: can place return PrologCodeEmptyList() here.
                        return ConvertBinaryListToCodeDOM(var);
                    }
                    if (var.Left == null && var.Right == null)
                    {
                        // 'Atom string'
                        if (var.Name[0] == '\'')
                        {
                            return new PrologCodeStringAtom(var.Name);
                        }
                        else if (Char.IsDigit(var.Name[0]))
                        {
                            // 1.234
                            if (var.Name.IndexOf('.') != -1)
                            {
                                return new PrologCodeFloatAtom(float.Parse(var.Name));
                            }
                            // 213
                            else
                            {
                                return new PrologCodeIntegerAtom(Int32.Parse(var.Name));
                            }
                        }
                        else if (var.Name == "_")
                        {
                            return new PrologCodeNilAtom();
                        }
                        // atom
                        else
                        {
                            
                            return new PrologCodeConstantAtom(var.Name);
                        }
                    }
                    else if (var.Left != null && var.Right != null)
                    {
                        PrologCodePredicate infixPredicate = new PrologCodePredicate(var.Name);
                        infixPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(var.Left));
                        infixPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(var.Right));
                        return infixPredicate;
                    }
                    else if (var.Left == null && var.Right != null)
                    {
                        PrologCodePredicate prefixPredicate = new PrologCodePredicate(var.Name);
                        prefixPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(var.Right));
                        return prefixPredicate;
                    }
                    else if (var.Left != null && var.Right == null)
                    {
                        PrologCodePredicate postfixPredicate = new PrologCodePredicate(var.Name);
                        postfixPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(var.Left));
                        return postfixPredicate;
                    }

                }
                // atom(a,X,atom(X)).
                else
                {
                    PrologCodePredicate functor = new PrologCodePredicate(var.Name);
                    ArrayList arguments = new ArrayList();
                    var.Flatten((BinaryTree)var.Arguments[0], ref arguments);
                    foreach (BinaryTree a in arguments)
                    {
                        functor.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(a));
                    }
                    return functor;
                }

            }
            return null;
        }

        private PrologCodeTerm ConvertGoalBinaryTreeToCodeDOM(BinaryTree goal)
        {
            if (goal.Name == ".")
            {
                return ConvertBinaryListToCodeDOM(goal);
            }
            else if (Char.IsUpper(goal.Name[0]))  // Goal is a variable
            {
                return new PrologCodeVariable(goal.Name);
            }
            else
            {
                PrologCodePredicate goalPredicate = new PrologCodePredicate(goal.Name);
                ArrayList gargs = new ArrayList();
                goal.Flatten(goal, ref gargs);
                goalPredicate.IsMethod = IsMethod(goal.Name, gargs.Count);
                goalPredicate.MethodInfo = GetMethodInfo(goal.Name);

                if (goal.Arguments != null && goal.Arguments.Count != 0)
                {
                    // Example:
                    // goal(X,a).
                    ArrayList arguments = new ArrayList();
                    goal.Flatten((BinaryTree)goal.Arguments[0], ref arguments);
                    foreach (BinaryTree a in arguments)
                    {
                        goalPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(a));
                    }
                    return goalPredicate;
                }
                else
                {
                    // X = a (goal is '=')  
                    if (goal.Left != null && goal.Right != null)
                    {
                        goalPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(goal.Left));
                        goalPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(goal.Right));
                        return goalPredicate;
                    }
                    // [] + foo.
                    if (goal.Left == null && goal.Right != null)
                    {
                        goalPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(goal.Right));
                        return goalPredicate;
                    }
                    // X + []
                    if (goal.Left != null && goal.Right == null)
                    {
                        goalPredicate.Arguments.Add(ConvertGoalVariableBinaryTreeToCodeDOM(goal.Left));
                        return goalPredicate;
                    }
                    if (goal.Left == null && goal.Right == null)
                    {
                        return goalPredicate;
                    }
                }
            }
            return null;

        }

        private PrologCodeTerm ConvertBinaryListToCodeDOM(BinaryTree l)
        {
            BinaryList list = (BinaryList)l;

            if (list.Head == null && list.Tail == null)
            {
                return new PrologCodeEmptyList();
            }

            PrologCodeNonEmptyList NEList = null;
            if (list.Head.Name == ".")
            {
                NEList = new PrologCodeNonEmptyList(ConvertBinaryListToCodeDOM(list.Head));
            }
            else
            {
                NEList = new PrologCodeNonEmptyList(ConvertGoalVariableBinaryTreeToCodeDOM(list.Head));
            }

            // Check the tail
            if (list.Tail.Name == ".")
            {
                NEList.Tail = ConvertBinaryListToCodeDOM(list.Tail);
            }
            else
            {
                NEList.Tail = ConvertGoalVariableBinaryTreeToCodeDOM(list.Tail);
            }
            return NEList;

        }

        public PrologCodeTerm ConvertBinaryTreeToCodeDOM(BinaryTree tree)
        {
            // Clause
            if (tree.Name == ":-")
            {
                PrologCodeClause term = new PrologCodeClause();
                if (tree.Left != null)
                {
                    ArrayList goals = new ArrayList();
                    tree.Flatten(tree.Right, ref goals);
                    foreach (BinaryTree goal in goals)
                    {
                        term.Goals.Add(ConvertGoalBinaryTreeToCodeDOM(goal));
                    }
                    term.Head = (PrologCodePredicate)ConvertBinaryTreeToCodeDOM(tree.Left);
                    return term;
                }
                // Headless clause
                else
                {  
                    // process headless clause here
                    PrologCodeHeadlessClause hClause = new PrologCodeHeadlessClause();
                    ArrayList goals = new ArrayList();
                    tree.Flatten(tree.Right, ref goals);
                    foreach (BinaryTree goal in goals)
                    {
                        hClause.Goals.Add(ConvertGoalBinaryTreeToCodeDOM(goal));
                    }
                    return hClause;
                }
            }
            else if (tree.Name == ".")
            {
                return ConvertBinaryListToCodeDOM(tree);
               
            }
            // Variable
            else if (Char.IsUpper(tree.Name[0]))
            {
                if (tree.Left != null || tree.Right != null)
                {
                    ParserError("Something was not parsed right. Variable has arity > 0", 0, 0);
                }
                PrologCodeVariable var = new PrologCodeVariable(tree.Name);
                return var;
            }
            else
            {
                return ConvertGoalBinaryTreeToCodeDOM(tree);
            }
            //return null;
        }

        public PrologScanner Scanner
        {
            get { return _scanner; }
            set { _scanner = value; }
        }

        public ArrayList Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        // determines whether its a method or not
        private bool IsMethod(string name, int arity)
        {
            foreach (PrologCodeMethod method in _codeUnit.Methods)
            {
                if (name == method.PredicateName && method.Arguments.Count == arity)
                {
                    return true;
                }
            }
            return false;
        }

        private PrologCodeMethod GetMethodInfo(string name)
        {
            foreach (PrologCodeMethod method in _codeUnit.Methods)
            {
                if (method.PredicateName == name)
                {
                    return method;
                }
            }
            return null;
        }
    }
    #region BinaryTree internal class
    public class BinaryTree
    {
        public BinaryTree _left;
        public BinaryTree _right;
        public string _name;
        public ArrayList _arguments = null;

        public BinaryTree(string name)
        {
            _name = name;
            _left = null;
            _right = null;
        }
        public BinaryTree(string name, BinaryTree left, BinaryTree right)
        {
            _name = name;
            _left = left;
            _right = right;
            _arguments = new ArrayList();
        }

        public BinaryTree(string name, ArrayList arguments)
        {
            _left = null;
            _right = null;
            _arguments = arguments;
            _name = name;
        }

        public BinaryTree(string name, BinaryTree left)
        {
            _name = name;
            _left = left;
            _arguments = null;
        }

        /* converts a Tree to an array list */
        public void Flatten(BinaryTree t, ref ArrayList args)
        {
            if (t.Name == ",")
            {
                args.Add(t.Left);
                if (t.Right.Name == ",")
                {
                    Flatten(t.Right, ref args);
                }
                else
                {
                    args.Add(t.Right);
                }
            }
            else
            {
                args.Add(t);
            }
        }


        public BinaryTree Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public BinaryTree Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public ArrayList Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
    #endregion

    #region BinaryList internal class
    public class BinaryList : BinaryTree
    {
        private BinaryTree _head = null;
        private BinaryTree _tail = null;
        private bool _empty = true;
        public BinaryList() : base(".", null, null)
        {
        }

        public BinaryList(BinaryTree head, BinaryTree tail) : base(".", null, null)
        {
            _head = head;
            _tail = tail;
            _empty = false;
        }

        public BinaryTree Head
        {
            get { return _head; }
            set { _head = value; }
        }

        public BinaryTree Tail
        {
            get { return _tail; }
            set { _tail = value; }
        }

        public bool IsEmpty
        {
            get { return _empty; }
        }
    }
    #endregion
}