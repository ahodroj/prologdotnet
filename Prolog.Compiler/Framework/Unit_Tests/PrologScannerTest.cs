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
    public class PrologScannerTest
    {
        private PrologScanner scanner = null;
        private ArrayList filesToDelete = new ArrayList();


        [SetUp]
        public void TestSetup()
        {
            // Do nothing...
        }
       
        [TearDown]
        public void TestEnd()
        {
            foreach (string filename in filesToDelete)
            {
                File.Delete("C:\\" + filename);
            }
        }

        public void Write(string filename, string s)
        {
            StreamWriter streamWriter = new StreamWriter("C:\\" + filename,false);
            streamWriter.WriteLine(s);
            streamWriter.Close();
            filesToDelete.Add("filename");
            
        }

        [Test]
        public void StringTest()
        {
            Write("string.txt", " 'This is a string'  ");
            
            StreamReader stream = new StreamReader("C:\\string.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);
            stream.Close();
         
        }

        [Test]
        public void AtomTest()
        {
            Write("atom.txt", " atom  ");
            StreamReader stream = new StreamReader("C:\\atom.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void VariableTest()
        {
            Write("var.txt", "Father  ");
            StreamReader stream = new StreamReader("C:\\var.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.VARIABLE, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void LParenTest()
        {
            Write("paren.txt", "(  ");
            StreamReader stream = new StreamReader("C:\\paren.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.LPAREN, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void RParenTest()
        {
            Write("rparen.txt", "     )  ");
            StreamReader stream = new StreamReader("C:\\rparen.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.RPAREN, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void DotTest()
        {
            Write("dot.txt", " . ");
            StreamReader stream = new StreamReader("C:\\dot.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.DOT, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void LBracketTest()
        {
            Write("bra.txt", " [  ");
            StreamReader stream = new StreamReader("C:\\bra.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.LBRACKET, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void RBracketTest()
        {
            Write("rbra.txt", " ]  ");
            StreamReader stream = new StreamReader("C:\\rbra.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.RBRACKET, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void ListSepTest()
        {
            Write("sep.txt", " |  ");
            StreamReader stream = new StreamReader("C:\\sep.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.LIST_SEP, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void CommaTest()
        {
            Write("comma.txt", " ,  ");
            StreamReader stream = new StreamReader("C:\\comma.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.COMMA, scanner.Current.Kind);
            stream.Close();
        }

        [Test]
        public void CurrentTest()
        {
            Write("current.txt", "scary Variable  ");
            StreamReader stream = new StreamReader("C:\\current.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            scanner.Next();
            Assert.AreEqual(PrologToken.VARIABLE, scanner.Current.Kind);
            
            stream.Close();
        }

        [Test]
        public void LookaheadTest()
        {
            Write("lookahead.txt", "scary Variable  ");
            StreamReader stream = new StreamReader("C:\\lookahead.txt");
            scanner = new PrologScanner(stream);
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            Assert.AreEqual(PrologToken.VARIABLE, scanner.Lookahead.Kind);

            stream.Close();
        }

        [Test]
        public void FactTokensRule()
        {
            Write("fact_tokens.txt", "male(ali,hodroj,X,'Fine not really!',[A|B]).");
            StreamReader stream = new StreamReader("C:\\fact_tokens.txt");
            scanner = new PrologScanner(stream);
            // male
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            // (
            scanner.Next();
            Assert.AreEqual(PrologToken.LPAREN, scanner.Current.Kind);

            // ali
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            // ,
            scanner.Next();
            Assert.AreEqual(PrologToken.COMMA, scanner.Current.Kind);

            // hodroj
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            // ,
            scanner.Next();
            Assert.AreEqual(PrologToken.COMMA, scanner.Current.Kind);

            // X
            scanner.Next();
            Assert.AreEqual(PrologToken.VARIABLE, scanner.Current.Kind);

            // ,
            scanner.Next();
            Assert.AreEqual(PrologToken.COMMA, scanner.Current.Kind);

            // 'Fine Not Really!'
            scanner.Next();
            Assert.AreEqual(PrologToken.ATOM, scanner.Current.Kind);

            // ,
            scanner.Next();
            Assert.AreEqual(PrologToken.COMMA, scanner.Current.Kind);


            // [
            scanner.Next();
            Assert.AreEqual(PrologToken.LBRACKET, scanner.Current.Kind);

            // A
            scanner.Next();
            Assert.AreEqual(PrologToken.VARIABLE, scanner.Current.Kind);

            // |
            scanner.Next();
            Assert.AreEqual(PrologToken.LIST_SEP, scanner.Current.Kind);

            // B
            scanner.Next();
            Assert.AreEqual(PrologToken.VARIABLE, scanner.Current.Kind);

            // ]
            scanner.Next();
            Assert.AreEqual(PrologToken.RBRACKET, scanner.Current.Kind);

            // )
            scanner.Next();
            Assert.AreEqual(PrologToken.RPAREN, scanner.Current.Kind);


            // .
            scanner.Next();
            Assert.AreEqual(PrologToken.DOT, scanner.Current.Kind);

            stream.Close();
        }

    }
}
