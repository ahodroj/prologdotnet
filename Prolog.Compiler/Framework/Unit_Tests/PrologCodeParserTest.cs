using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Axiom.Compiler.Framework;
using NUnit.Framework;

namespace Axiom.Compiler.Framework.Unit_Tests
{
    [TestFixture]
    public class PrologCodeParserTest
    {
        private ArrayList filesToDelete = new ArrayList();

        [SetUp]
        public void TestSetup()
        {
            
        }

        [TearDown]
        public void TestEnd()
        {
            
        }

        private void Write(string filename, string s)
        {
            Console.WriteLine(s);
            StreamWriter sw = new StreamWriter("C:\\" + filename, false);
            sw.WriteLine(s);
            sw.Close();
         
        }
        [Test]
        public void Parse_Fact_no_Args()
        {
            // Try to parse 'predicate.'
            Write("factnoargs.txt", "predicate.");

            StreamReader sr = new StreamReader("C:\\factnoargs.txt");

            PrologCodeParser parser = new PrologCodeParser();
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();

            // Expect: BinaryTree("predicate", null, null, null);
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            Assert.IsNull(ast.Arguments);
        }

        [Test]
        public void Parse_Fact_with_Atom_Arg()
        {
            // Try to parse 'predicate.'
            Write("factnoargs.txt", "predicate(ali).");

            StreamReader sr = new StreamReader("C:\\factnoargs.txt");

            PrologCodeParser parser = new PrologCodeParser();
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();

            // Expect: BinaryTree("predicate", null, null, null);
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(1, args.Count);
           
        }

        [Test]
        public void Parse_Fact_with_Variable_Arg()
        {
            // Try to parse 'predicate.'
            Write("factnoargs.txt", "predicate(X).");

            StreamReader sr = new StreamReader("C:\\factnoargs.txt");

            PrologCodeParser parser = new PrologCodeParser();
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();

            // Expect: BinaryTree("predicate", null, null, null);
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(1, args.Count);

        }

        [Test]
        public void Parse_Fact_with_2_Args()
        {
            // Try to parse 'predicate.'
            Write("factnoargs.txt", "predicate(X,X).");

            StreamReader sr = new StreamReader("C:\\factnoargs.txt");

            PrologCodeParser parser = new PrologCodeParser();
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();

            // Expect: BinaryTree("predicate", null, null, null);
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(2, args.Count);

        }

        [Test]
        public void Parse_Fact_with_3_Args()
        {
            // Try to parse 'predicate.'
            Write("factnoargs.txt", "predicate(X,X,X).");

            StreamReader sr = new StreamReader("C:\\factnoargs.txt");

            PrologCodeParser parser = new PrologCodeParser();
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();

            // Expect: BinaryTree("predicate", null, null, null);
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(3, args.Count);

        }

        [Test]
        public void Parse_Fact_with_Mixed_Args()
        {
            BinaryTree ast = PrologTerm("predicate(ali,X,s(X)).");
            
            
            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(3, args.Count);

        }

        [Test]
        public void Parse_Fact_with_ListArg()
        {
           BinaryTree ast = PrologTerm("predicate([X]).");


            Assert.AreEqual("predicate", ast.Name);
            Assert.IsNull(ast.Left);
            Assert.IsNull(ast.Right);
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Arguments[0], ref args);
            Assert.AreEqual(1, args.Count);

        }

        [Test]
        public void Parse_Clause_1_Goal_Arity0()
        {
            BinaryTree ast = PrologTerm("ali :- samir.");


            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("samir", ast.Right.Name);
           

        }

        [Test]
        public void Parse_Clause_1_Goal_Arity1()
        {
            BinaryTree ast = PrologTerm("ali(X) :- samir.");
            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("samir", ast.Right.Name);
            Assert.AreEqual(1, ast.Left.Arguments.Count);
            Assert.AreEqual("X", ((BinaryTree)ast.Left.Arguments[0]).Name);
        }

        [Test]
        public void TestPredicate()
        {
            BinaryTree ast = PrologTerm("ali(X) :- samir.");
            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("samir", ast.Right.Name);
            Assert.AreEqual(1, ast.Left.Arguments.Count);
            Assert.AreEqual("X", ((BinaryTree)ast.Left.Arguments[0]).Name);
        }

      

        [Test]
        public void Parse_Clause_1_Goal_Arity2()
        {
            BinaryTree ast = PrologTerm("ali(X,Y) :- samir.");
            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("samir", ast.Right.Name);
            Assert.AreEqual(1, ast.Left.Arguments.Count);
            BinaryTree args = (BinaryTree)ast.Left.Arguments[0];
            Assert.AreEqual(",", args.Name);
            Assert.AreEqual("X", args.Left.Name);
            Assert.AreEqual("Y", args.Right.Name);
        }

        [Test]
        public void Parse_Clause_1_Goal_Arity3()
        {
            BinaryTree ast = PrologTerm("ali(X,Y,Z) :- samir.");
            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("samir", ast.Right.Name);
            Assert.AreEqual(1, ast.Left.Arguments.Count);
            BinaryTree args = (BinaryTree)ast.Left.Arguments[0];
            Assert.AreEqual(",", args.Name);
            Assert.AreEqual("X", args.Left.Name);
            Assert.AreEqual(",", args.Right.Name);
            BinaryTree rightArgs = (BinaryTree)args.Right;
            Assert.AreEqual("Y", rightArgs.Left.Name);
            Assert.AreEqual("Z", rightArgs.Right.Name);
        }

       


        [Test]
        public void Parse_Clause_1_Goal_Arity4()
        {

            BinaryTree ast = PrologTerm("ali(X1,X2,X3,X4) :- samir.");
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Left.Arguments[0], ref args);
            Assert.AreEqual(4, args.Count);
        }

        [Test]
        public void Parse_Clause_1_Goal_Arity5()
        {

            BinaryTree ast = PrologTerm("ali(X1,X2,X3,X4,X5) :- samir.");
            ArrayList args = new ArrayList();
            ast.Flatten((BinaryTree)ast.Left.Arguments[0], ref args);
            Assert.AreEqual(5, args.Count);
        }

        private BinaryTree PrologTerm(string s)
        {
            
            // Try to parse 'predicate.'
            Write("parsertest.txt", s);

            StreamReader sr = new StreamReader("C:\\parsertest.txt");

            PrologCodeParser parser = new PrologCodeParser();
            
            parser.Scanner = new PrologScanner(sr);

            BinaryTree ast = parser.Term(1200);
            sr.Close();
            File.Delete("C:\\parsertest.txt");
            return ast;
        }

    }
}
