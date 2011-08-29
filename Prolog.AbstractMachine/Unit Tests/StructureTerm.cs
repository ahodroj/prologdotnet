

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _StructureTerm
    {

        [Test]
        public void IsReference()
        {
            StructureTerm t = new StructureTerm();
            AbstractTerm a = new StructureTerm();
            Assert.IsFalse(t.IsReference);
            Assert.IsFalse(a.IsReference);

        }

        [Test]
        public void IsConstant()
        {
            StructureTerm t = new StructureTerm();
            AbstractTerm a = new StructureTerm();
            Assert.IsFalse(t.IsConstant);
            Assert.IsFalse(a.IsConstant);

        }

        [Test]
        public void IsStructure()
        {
            StructureTerm t = new StructureTerm();
            AbstractTerm a = new StructureTerm();
            Assert.IsTrue(t.IsStructure);
            Assert.IsTrue(a.IsStructure);
        }

        [Test]
        public void IsList()
        {
            StructureTerm t = new StructureTerm();
            AbstractTerm a = new StructureTerm();
            Assert.IsFalse(t.IsList);
            Assert.IsFalse(a.IsList);
        }

        [Test]
        public void IsObject()
        {
            StructureTerm t = new StructureTerm();
            AbstractTerm a = new StructureTerm();
            Assert.IsFalse(t.IsObject);
            Assert.IsFalse(a.IsObject);

        }

        [Test]
        public void StructureTerm()
        {
           
        }

        [Test]
        public void Arity()
        {
          
        }

        [Test]
        public void Name()
        {
           
        }

        [Test]
        public void Data()
        {
            StructureTerm t = new StructureTerm();
            Assert.IsNull(t.Data());
        }



       
       

       
       
       

        [Test]
        public void Indexing()
        {
            StructureTerm s = new StructureTerm("s", 2);

            s.Next = new ConstantTerm("ali");
            s.Next.Next = new ConstantTerm("samir");

            Assert.AreEqual("ali", s[0].Data());
            Assert.AreEqual("samir", s[1].Data());
        }
        // structures
        [Test]
        public void Unify_str_ref()
        {
            AbstractTerm term = new AbstractTerm();
            StructureTerm s = new StructureTerm("s", 2);

            Assert.IsTrue(s.Unify(term));

            Assert.AreEqual(term.Arity, s.Arity);
            Assert.AreEqual(term.Name, s.Name);

           
        }

        [Test]
        public void Unify_str_con()
        {
            ConstantTerm term = new ConstantTerm("ali");
            StructureTerm s = new StructureTerm("s", 2);

            Assert.IsFalse(s.Unify(term));
        }

        [Test]
        public void Unify_str_eq_str()
        {
            StructureTerm s1 = new StructureTerm("s", 2);
            s1.Next = new ConstantTerm("ali");
            s1.Next.Next = new ConstantTerm("samir");

            StructureTerm s2 = new StructureTerm("s", 2);
            s2.Next = new ConstantTerm("ali");
            s2.Next.Next = new ConstantTerm("samir");

            Assert.IsTrue(s1.Unify(s2));
        }

        [Test]
        public void Unify_str_ne_str()
        {
            StructureTerm s1 = new StructureTerm("s", 2);
            s1.Next = new ConstantTerm("ali");
            s1.Next.Next = new ConstantTerm("samir");

            StructureTerm s2 = new StructureTerm("s", 2);
            s2.Next = new ConstantTerm("samir");
            s2.Next.Next = new ConstantTerm("samir");

            Assert.IsFalse(s1.Unify(s2));
        }

        [Test]
        public void Unify_str_ref_str()
        {
            StructureTerm s1 = new StructureTerm("s", 1);
            s1.Next = new AbstractTerm();

            StructureTerm s2 = new StructureTerm("s", 1);
            s2.Next = new ConstantTerm("ali");

            Assert.IsTrue(s1.Unify(s2));

            Assert.AreEqual(s1[0].Data(), s2[0].Data());
        }

        [Test]
        public void Unify_str_lis()
        {
            StructureTerm s = new StructureTerm("s", 2);
            ListTerm l = new ListTerm();

            Assert.IsFalse(s.Unify(l));
        }



    }
}