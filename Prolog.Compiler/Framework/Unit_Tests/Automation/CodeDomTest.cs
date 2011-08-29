using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Axiom.Compiler.Framework;
using Axiom.Compiler.CodeObjectModel;
using NUnit.Framework;

namespace Automation
{
    [TestFixture]
    public class CodeDOMTest
    {
        [SetUp]
        public void TestSetup()
        {
            // Do nothing...
        }

        [TearDown]
        public void TestEnd()
        {
           
        }


        [Test]
        public void Testmethod()
        {
            string expected = "";
            string result = "";

            StreamReader sr = new StreamReader("C:\\codedomconfig.txt");
            string filename = sr.ReadLine();
            sr.Close();
            GetFile("C:\\source\\" + filename, ref expected, ref result);

            Assert.AreEqual(expected, result);

        }

        private void GetFile(string filename, ref string expected, ref string result)
        {
 
            StreamReader sr = new StreamReader(filename);

            PrologCodeParser parser = new PrologCodeParser();

            parser.Scanner = new PrologScanner(sr);

            PrologCodeTerm term = parser.ConvertBinaryTreeToCodeDOM(parser.Term(1200));
            sr.Close();

            StreamReader codefile = new StreamReader(filename);
            string originalCode = codefile.ReadLine();
            codefile.Close();
            string exp = originalCode.Replace(" ", "").Replace(".", "");
            string res = term.ToString().Replace(" ", "").Replace(".", "");

            expected = exp;
            result = res;
        }
    }
}
