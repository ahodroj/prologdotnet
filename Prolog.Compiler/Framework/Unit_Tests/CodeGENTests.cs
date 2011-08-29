using System;
using NUnit.Framework;
using System.IO;
using Axiom.Compiler.Framework;
using Axiom.Compiler.CodeObjectModel;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;
using System.Collections;
using System.Diagnostics;

namespace Automation
{
    using NUnit.Framework;

    [TestFixture]
    public class CodeGenTests
    {
        public void WriteFile(string name, string f)
        {
            StreamWriter sr = new StreamWriter(name);
            sr.Write(f);
            sr.Close();
        }

  [Test]
  public void Source_14_834()
  {
            Assert.IsTrue(CompareCode(
                ParseAndGenerate("test.pro"),
                AbstractCode("test.wam")
                ));
           
 }

  
        
        private ArrayList AbstractCode(string filename)
        {
            ArrayList program = new ArrayList();
            StreamReader sr = new StreamReader("C:\\wamsource\\" + filename);
            string txt = "";
            while ((txt = sr.ReadLine()) != null)
            {
                program.Add(txt);
            }
            sr.Close();

            return program;
        }

        private bool CompareCode(ArrayList p1, ArrayList p2)
        {
            
            if (p1.Count != p2.Count)
            {
                Console.WriteLine("Number of instructions does not match! Expected: {0}, Result: {1}", p2.Count, p1.Count);
                return false;
            }
            bool result = true;
            for (int i = 0; i < p1.Count; i++)
            {
                string p_1 = (string)p1[i];
                string p_2 = (string)p2[i];
                if (!p_1.Equals(p_2))
                {
                    Console.WriteLine("FAIL: '{0}' != '{1}'", (string)p1[i],
                        (string)p2[i]);
                    result = false;
                }
            }
            Console.WriteLine("\n\n");
            return result;
        }

        private bool CompareInstructions(AbstractInstruction i1, AbstractInstruction i2)
        {
            return i1.ToString() == i2.ToString();
        }
        private ArrayList ParseAndGenerate(string name)
        {
            throw new Exception("Not implemented.");
        }
    }
}