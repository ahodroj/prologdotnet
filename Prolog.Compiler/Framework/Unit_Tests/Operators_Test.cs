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
    public class Operators_Test
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


        #region Infix Tests
        
        //[Test]
        //public void Infix1()
        //{
        //    Console.WriteLine("Testing infix :- ");
        //    BinaryTree ast = PrologTerm("ali :- 1 :- 2.");

        //    Assert.AreEqual(":-", ast.Name);
        //    Assert.AreEqual("ali", ast.Left.Name);
        //    Assert.AreEqual(":-", ast.Right.Name);
        //    Assert.AreEqual("1", ast.Right.Left.Name);
        //    Assert.AreEqual("2", ast.Right.Right.Name);
        //}
        

        //[Test]
        //public void Infix2()
        //{
        //    Console.WriteLine("Testing infix --> ");
        //    BinaryTree ast = PrologTerm("ali :- 1 --> 2.");

        //    Assert.AreEqual(":-", ast.Name);
        //    Assert.AreEqual("ali", ast.Left.Name);
        //    Assert.AreEqual("-->", ast.Right.Name);
        //    Assert.AreEqual("1", ast.Right.Left.Name);
        //    Assert.AreEqual("2", ast.Right.Right.Name);
        //}
        

        [Test]
        public void Infix3()
        {
            Console.WriteLine("Testing infix <= ");
            BinaryTree ast = PrologTerm("ali :- 1 <= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("<=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix4()
        {
            Console.WriteLine("Testing infix <-> ");
            BinaryTree ast = PrologTerm("ali :- 1 <-> 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("<->", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix5()
        {
            Console.WriteLine("Testing infix <- ");
            BinaryTree ast = PrologTerm("ali :- 1 <- 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("<-", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix6()
        {
            Console.WriteLine("Testing infix until ");
            BinaryTree ast = PrologTerm("ali :- 1 until 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("until", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix7()
        {
            Console.WriteLine("Testing infix unless ");
            BinaryTree ast = PrologTerm("ali :- 1 unless 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("unless", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix8()
        {
            Console.WriteLine("Testing infix from ");
            BinaryTree ast = PrologTerm("ali :- 1 from 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("from", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix9()
        {
            Console.WriteLine("Testing infix =:= ");
            BinaryTree ast = PrologTerm("ali :- 1 =:= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("=:=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix10()
        {
            Console.WriteLine("Testing infix =\\= ");
            BinaryTree ast = PrologTerm("ali :- 1 =\\= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("=\\=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix11()
        {
            Console.WriteLine("Testing infix < ");
            BinaryTree ast = PrologTerm("ali :- 1 < 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("<", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix12()
        {
            Console.WriteLine("Testing infix >= ");
            BinaryTree ast = PrologTerm("ali :- 1 >= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(">=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix13()
        {
            Console.WriteLine("Testing infix > ");
            BinaryTree ast = PrologTerm("ali :- 1 > 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(">", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix14()
        {
            Console.WriteLine("Testing infix =< ");
            BinaryTree ast = PrologTerm("ali :- 1 =< 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("=<", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix15()
        {
            Console.WriteLine("Testing infix is ");
            BinaryTree ast = PrologTerm("ali :- 1 is 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("is", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix16()
        {
            Console.WriteLine("Testing infix =.. ");
            BinaryTree ast = PrologTerm("ali :- 1 =.. 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("=..", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix17()
        {
            Console.WriteLine("Testing infix == ");
            BinaryTree ast = PrologTerm("ali :- 1 == 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("==", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix18()
        {
            Console.WriteLine("Testing infix \\== ");
            BinaryTree ast = PrologTerm("ali :- 1 \\== 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("\\==", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix19()
        {
            Console.WriteLine("Testing infix = ");
            BinaryTree ast = PrologTerm("ali :- 1 = 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix20()
        {
            Console.WriteLine("Testing infix \\= ");
            BinaryTree ast = PrologTerm("ali :- 1 \\= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("\\=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix21()
        {
            Console.WriteLine("Testing infix @< ");
            BinaryTree ast = PrologTerm("ali :- 1 @< 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("@<", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix22()
        {
            Console.WriteLine("Testing infix @>= ");
            BinaryTree ast = PrologTerm("ali :- 1 @>= 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("@>=", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix23()
        {
            Console.WriteLine("Testing infix @> ");
            BinaryTree ast = PrologTerm("ali :- 1 @> 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("@>", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix24()
        {
            Console.WriteLine("Testing infix @=< ");
            BinaryTree ast = PrologTerm("ali :- 1 @=< 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("@=<", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix25()
        {
            Console.WriteLine("Testing infix mod ");
            BinaryTree ast = PrologTerm("ali :- 1 mod 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("mod", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix26()
        {
            Console.WriteLine("Testing infix : ");
            BinaryTree ast = PrologTerm("ali :- 1 : 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(":", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix27()
        {
            Console.WriteLine("Testing infix , ");
            BinaryTree ast = PrologTerm("ali :- 1 , 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(",", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix28()
        {
            Console.WriteLine("Testing infix ; ");
            BinaryTree ast = PrologTerm("ali :- 1 ; 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(";", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix29()
        {
            Console.WriteLine("Testing infix -> ");
            BinaryTree ast = PrologTerm("ali :- 1 -> 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("->", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        //[Test]
        //public void Infix30()
        //{
        //    Console.WriteLine("Testing infix . ");
        //    BinaryTree ast = PrologTerm("ali :- 1 . 2.");

        //    Assert.AreEqual(":-", ast.Name);
        //    Assert.AreEqual("ali", ast.Left.Name);
        //    Assert.AreEqual(".", ast.Right.Name);
        //    Assert.AreEqual("1", ast.Right.Left.Name);
        //    Assert.AreEqual("2", ast.Right.Right.Name);
        //}
        

        [Test]
        public void Infix31()
        {
            Console.WriteLine("Testing infix >> ");
            BinaryTree ast = PrologTerm("ali :- 1 >> 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual(">>", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix32()
        {
            Console.WriteLine("Testing infix ^ ");
            BinaryTree ast = PrologTerm("ali :- 1 ^ 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("^", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix33()
        {
            Console.WriteLine("Testing infix + ");
            BinaryTree ast = PrologTerm("ali :- 1 + 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("+", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix34()
        {
            Console.WriteLine("Testing infix - ");
            BinaryTree ast = PrologTerm("ali :- 1 - 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("-", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix35()
        {
            Console.WriteLine("Testing infix \\/ ");
            BinaryTree ast = PrologTerm("ali :- 1 \\/ 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("\\/", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix36()
        {
            Console.WriteLine("Testing infix /\\ ");
            BinaryTree ast = PrologTerm("ali :- 1 /\\ 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("/\\", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix37()
        {
            Console.WriteLine("Testing infix * ");
            BinaryTree ast = PrologTerm("ali :- 1 * 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("*", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix38()
        {
            Console.WriteLine("Testing infix / ");
            BinaryTree ast = PrologTerm("ali :- 1 / 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("/", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix39()
        {
            Console.WriteLine("Testing infix div ");
            BinaryTree ast = PrologTerm("ali :- 1 div 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("div", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix40()
        {
            Console.WriteLine("Testing infix // ");
            BinaryTree ast = PrologTerm("ali :- 1 // 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("//", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

        [Test]
        public void Infix41()
        {
            Console.WriteLine("Testing infix << ");
            BinaryTree ast = PrologTerm("ali :- 1 << 2.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Left.Name);
            Assert.AreEqual("<<", ast.Right.Name);
            Assert.AreEqual("1", ast.Right.Left.Name);
            Assert.AreEqual("2", ast.Right.Right.Name);
        }
        

    #endregion

        #region Prefix Tests

        [Test]
        public void Prefix1()
        {
            Console.WriteLine("Testing prefix :- ");
            BinaryTree ast = PrologTerm(":- ali.");

            Assert.AreEqual(":-", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix2()
        {
            Console.WriteLine("Testing prefix ?- ");
            BinaryTree ast = PrologTerm("?- ali.");

            Assert.AreEqual("?-", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix3()
        {
            Console.WriteLine("Testing prefix gen ");
            BinaryTree ast = PrologTerm("gen ali.");

            Assert.AreEqual("gen", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix4()
        {
            Console.WriteLine("Testing prefix try ");
            BinaryTree ast = PrologTerm("try ali.");

            Assert.AreEqual("try", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix5()
        {
            Console.WriteLine("Testing prefix once ");
            BinaryTree ast = PrologTerm("once ali.");

            Assert.AreEqual("once", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix6()
        {
            Console.WriteLine("Testing prefix possible ");
            BinaryTree ast = PrologTerm("possible ali.");

            Assert.AreEqual("possible", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix7()
        {
            Console.WriteLine("Testing prefix side_effects ");
            BinaryTree ast = PrologTerm("side_effects ali.");

            Assert.AreEqual("side_effects", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix8()
        {
            Console.WriteLine("Testing prefix unit ");
            BinaryTree ast = PrologTerm("unit ali.");

            Assert.AreEqual("unit", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix9()
        {
            Console.WriteLine("Testing prefix visible ");
            BinaryTree ast = PrologTerm("visible ali.");

            Assert.AreEqual("visible", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix10()
        {
            Console.WriteLine("Testing prefix import ");
            BinaryTree ast = PrologTerm("import ali.");

            Assert.AreEqual("import", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix11()
        {
            Console.WriteLine("Testing prefix push ");
            BinaryTree ast = PrologTerm("push ali.");

            Assert.AreEqual("push", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix12()
        {
            Console.WriteLine("Testing prefix down ");
            BinaryTree ast = PrologTerm("down ali.");

            Assert.AreEqual("down", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix13()
        {
            Console.WriteLine("Testing prefix set ");
            BinaryTree ast = PrologTerm("set ali.");

            Assert.AreEqual("set", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix14()
        {
            Console.WriteLine("Testing prefix dynamic ");
            BinaryTree ast = PrologTerm("dynamic ali.");

            Assert.AreEqual("dynamic", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix15()
        {
            Console.WriteLine("Testing prefix + ");
            BinaryTree ast = PrologTerm("+ ali.");

            Assert.AreEqual("+", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix16()
        {
            Console.WriteLine("Testing prefix - ");
            BinaryTree ast = PrologTerm("- ali.");

            Assert.AreEqual("-", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix17()
        {
            Console.WriteLine("Testing prefix \\ ");
            BinaryTree ast = PrologTerm("\\ ali.");

            Assert.AreEqual("\\", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix18()
        {
            Console.WriteLine("Testing prefix @ ");
            BinaryTree ast = PrologTerm("@ ali.");

            Assert.AreEqual("@", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix19()
        {
            Console.WriteLine("Testing prefix @@ ");
            BinaryTree ast = PrologTerm("@@ ali.");

            Assert.AreEqual("@@", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix20()
        {
            Console.WriteLine("Testing prefix not ");
            BinaryTree ast = PrologTerm("not ali.");

            Assert.AreEqual("not", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix21()
        {
            Console.WriteLine("Testing prefix \\+ ");
            BinaryTree ast = PrologTerm("\\+ ali.");

            Assert.AreEqual("\\+", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix22()
        {
            Console.WriteLine("Testing prefix spy ");
            BinaryTree ast = PrologTerm("spy ali.");

            Assert.AreEqual("spy", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix23()
        {
            Console.WriteLine("Testing prefix nospy ");
            BinaryTree ast = PrologTerm("nospy ali.");

            Assert.AreEqual("nospy", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix24()
        {
            Console.WriteLine("Testing prefix ? ");
            BinaryTree ast = PrologTerm("? ali.");

            Assert.AreEqual("?", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix25()
        {
            Console.WriteLine("Testing prefix > ");
            BinaryTree ast = PrologTerm("> ali.");

            Assert.AreEqual(">", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }
        

        [Test]
        public void Prefix26()
        {
            Console.WriteLine("Testing prefix < ");
            BinaryTree ast = PrologTerm("< ali.");

            Assert.AreEqual("<", ast.Name);
            Assert.AreEqual("ali", ast.Right.Name);
            
        }


        #endregion

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
