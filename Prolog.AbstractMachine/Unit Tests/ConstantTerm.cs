

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{

    [TestFixture]
    public class _ConstantTerm
    {
        [Test]
        public void IsReference()
        {
            ConstantTerm t = new ConstantTerm();
            AbstractTerm a = new ConstantTerm();
            Assert.IsFalse(t.IsReference);
            Assert.IsFalse(a.IsReference);

        }

        [Test]
        public void IsConstant()
        {
            ConstantTerm t = new ConstantTerm();
            AbstractTerm a = new ConstantTerm();
            Assert.IsTrue(t.IsConstant);
            Assert.IsTrue(a.IsConstant);

        }

        [Test]
        public void IsStructure()
        {
            ConstantTerm t = new ConstantTerm();
            AbstractTerm a = new ConstantTerm();
            Assert.IsFalse(t.IsStructure);
            Assert.IsFalse(a.IsStructure);
        }

        [Test]
        public void IsList()
        {
            ConstantTerm t = new ConstantTerm();
            AbstractTerm a = new ConstantTerm();
            Assert.IsFalse(t.IsList);
            Assert.IsFalse(a.IsList);
        }

        [Test]
        public void IsObject()
        {
            ConstantTerm t = new ConstantTerm();
            AbstractTerm a = new ConstantTerm();
            Assert.IsFalse(t.IsObject);
            Assert.IsFalse(a.IsObject);

        }



        [Test]
        public void ConstantTerm()
        {
            ConstantTerm c = new ConstantTerm("Ali");

            Assert.AreEqual("Ali", c.Data());
        }

        

        [Test]
        public void Data()
        {
            ConstantTerm t = new ConstantTerm();
            Assert.IsNull(t.Data());
        }



        [Test]
        public void Bind()
        {
            ConstantTerm term = new ConstantTerm();
            ConstantTerm term2 = new ConstantTerm();

            term.Bind(term2);

            Assert.AreSame(term, term.Reference());

        }

       

        [Test]
        public void Dereference()
        {
            ConstantTerm term1 = new ConstantTerm();
            ConstantTerm term2 = new ConstantTerm();
            ConstantTerm term3 = new ConstantTerm();

            term2.Bind(term3);
            term1.Bind(term2);

            Assert.AreSame(term1, term1.Dereference());
        }

        [Test]
        public void Reference()
        {
            ConstantTerm term1 = new ConstantTerm();
            ConstantTerm term2 = new ConstantTerm();
            
            term1.Bind(term2);

            Assert.AreSame(term1, term1.Reference());
        }


        [Test]
        public void Unify_con_ref()
        {
            AbstractTerm term = new AbstractTerm();

            ConstantTerm con = new ConstantTerm("ali");

            Assert.IsTrue(con.Unify(term));

            Assert.AreSame(term.Data(), con.Data());
        }

        [Test]
        public void Unify_con_eq_con()
        {
            ConstantTerm con = new ConstantTerm("ali");
            ConstantTerm term = new ConstantTerm("ali");

            Assert.IsTrue(con.Unify(term));
        }

        [Test]
        public void Unify_con_ne_con()
        {
            ConstantTerm con = new ConstantTerm("ali");
            ConstantTerm term = new ConstantTerm("samir");

            Assert.IsFalse(con.Unify(term));

        }

        [Test]
        public void Unify_con_str()
        {
            ConstantTerm con = new ConstantTerm("ali");
            StructureTerm term = new StructureTerm("samir", 2);

            Assert.IsFalse(con.Unify(term));
        }

        [Test]
        public void Unify_con_lis()
        {
            ConstantTerm con = new ConstantTerm("ali");
            ListTerm term = new ListTerm();

            Assert.IsFalse(con.Unify(term));
        }

    }
}