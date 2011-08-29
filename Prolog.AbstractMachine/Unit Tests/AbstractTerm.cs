

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AbstractTerm
    {
        [Test]
        public void IsReference()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsTrue(t.IsReference);
            
        }

        [Test]
        public void IsConstant()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsFalse(t.IsConstant);
        }

        [Test]
        public void IsStructure()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsFalse(t.IsStructure);
        }

        [Test]
        public void IsList()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsFalse(t.IsList);
        }

        [Test]
        public void IsObject()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsFalse(t.IsObject);

        }

        [Test]
        public void Data()
        {
            AbstractTerm t = new AbstractTerm();
            Assert.IsNull(t.Data());
        }



        [Test]
        public void Bind()
        {
            AbstractTerm term = new AbstractTerm();
            AbstractTerm term2 = new AbstractTerm();

            term.Bind(term2);

            Assert.AreSame(term2, term.Reference());

        }

       

        [Test]
        public void Dereference()
        {
            AbstractTerm term1 = new AbstractTerm();
            AbstractTerm term2 = new AbstractTerm();
            AbstractTerm term3 = new AbstractTerm();

            term2.Bind(term3);
            term1.Bind(term2);

            Assert.AreSame(term3, term1.Dereference());
        }

        [Test]
        public void Reference()
        {
            AbstractTerm term1 = new AbstractTerm();
            AbstractTerm term2 = new AbstractTerm();

            term1.Bind(term2);

            Assert.AreSame(term2, term1.Reference());
        }

        [Test]
        public void Assign()
        {
            AbstractTerm term = new AbstractTerm();
            ConstantTerm con = new ConstantTerm("ali");

            term.Assign(con);

            Assert.AreSame(term.Dereference(), con);
            Assert.AreSame(term.Dereference(), con.Dereference());
            Assert.AreEqual(term.Data(), con.Data());
            Assert.AreSame(term.Reference(), con.Reference());
            Assert.IsFalse(term.IsList);
            Assert.IsFalse(term.IsObject);
            Assert.IsFalse(term.IsReference);
            Assert.IsFalse(term.IsStructure);
            Assert.IsTrue(term.IsConstant);
        }

        [Test]
        public void Unbind()
        {
            AbstractTerm term = new AbstractTerm();
            ConstantTerm con = new ConstantTerm("ali");

            term.Assign(con);

            term.Unbind();

            Assert.AreNotSame(term.Reference(), con.Reference());
        }


        // Unification tests

        [Test]
        public void Unify_ref_ref()
        {
            AbstractTerm term = new AbstractTerm();
            AbstractTerm another = new AbstractTerm();

            bool result = term.Unify(another);

            Assert.AreSame(term.Dereference(), another);
            Assert.IsTrue(result);
        }

        [Test]
        public void Unify_ref_con()
        {
            AbstractTerm term = new AbstractTerm();
            ConstantTerm con = new ConstantTerm();

            Assert.IsTrue(term.Unify(con));

            Assert.AreEqual(term.Data(), con.Data());
            Assert.AreSame(term.Reference(), con.Reference());
            Assert.IsTrue(term.IsConstant);
            Assert.IsFalse(term.IsReference);
        }

        [Test]
        public void Unify_ref_str()
        {
            AbstractTerm term = new AbstractTerm();
            StructureTerm con = new StructureTerm();

            Assert.IsTrue(term.Unify(con));

            Assert.AreSame(term.Reference(), con.Reference());
            Assert.IsTrue(term.IsStructure);
            Assert.IsFalse(term.IsReference);

        }

        [Test]
        public void Unify_ref_lis()
        {
            AbstractTerm term = new AbstractTerm();
            ListTerm con = new ListTerm();

            Assert.IsTrue(term.Unify(con));

            Assert.AreSame(term.Reference(), con.Reference());
            Assert.IsTrue(term.IsList);
            Assert.IsFalse(term.IsReference);
        }

        [Test]
        public void Unify_after_assignment()
        {
            AbstractTerm term = new AbstractTerm();
            ConstantTerm con = new ConstantTerm("test");

            term.Assign(con);

            Assert.AreEqual(term.Unify(con), con.Unify(term));
        }

    }
}