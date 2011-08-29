

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _ListTerm
    {

        [Test]
        public void IsReference()
        {
            ListTerm t = new ListTerm();
            AbstractTerm a = new ListTerm();
            Assert.IsFalse(t.IsReference);
            Assert.IsFalse(a.IsReference);

        }

        [Test]
        public void IsConstant()
        {
            ListTerm t = new ListTerm();
            AbstractTerm a = new ListTerm();
            Assert.IsFalse(t.IsConstant);
            Assert.IsFalse(a.IsConstant);

        }

        [Test]
        public void IsStructure()
        {
            ListTerm t = new ListTerm();
            AbstractTerm a = new ListTerm();
            Assert.IsFalse(t.IsStructure);
            Assert.IsFalse(a.IsStructure);
        }

        [Test]
        public void IsList()
        {
            ListTerm t = new ListTerm();
            AbstractTerm a = new ListTerm();
            Assert.IsTrue(t.IsList);
            Assert.IsTrue(a.IsList);
        }

        [Test]
        public void IsObject()
        {
            ListTerm t = new ListTerm();
            AbstractTerm a = new ListTerm();
            Assert.IsFalse(t.IsObject);
            Assert.IsFalse(a.IsObject);

        }

        [Test]
        public void ListTerm()
        {
            ListTerm list = new ListTerm();

            Assert.IsNotNull(list);
        }

        

        [Test]
        public void Data()
        {
            ListTerm t = new ListTerm();
            Assert.IsNull(t.Data());
        }



        [Test]
        public void Bind()
        {
            ListTerm term = new ListTerm();
            ListTerm term2 = new ListTerm();

            term.Bind(term2);

            Assert.AreSame(term, term.Reference());

        }

       

        [Test]
        public void Dereference()
        {
            ListTerm term1 = new ListTerm();
            ListTerm term2 = new ListTerm();
            ListTerm term3 = new ListTerm();

            term2.Bind(term3);
            term1.Bind(term2);

            Assert.AreSame(term1, term1.Dereference());
        }

        [Test]
        public void Reference()
        {
            ListTerm term1 = new ListTerm();
            ListTerm term2 = new ListTerm();

            term1.Bind(term2);

            Assert.AreSame(term1, term1.Reference());
        }

        // List tests
        [Test]
        public void Unify_lis_ref()
        {
            ListTerm list = new ListTerm();
            list.Next = new ConstantTerm("ali");
            list.Next.Next = new ConstantTerm("samir");

            AbstractTerm term = new AbstractTerm();

            Assert.IsTrue(list.Unify(term));

            Assert.AreSame(list.Head, term.Head);
            Assert.AreSame(list.Tail, term.Tail);
        }

        [Test]
        public void Unify_lis_con()
        {
            ListTerm list = new ListTerm();
            ConstantTerm con = new ConstantTerm();

            Assert.IsFalse(list.Unify(con));
        }

        [Test]
        public void Unify_lis_str()
        {
            ListTerm list = new ListTerm();
            StructureTerm con = new StructureTerm("s", 2);

            Assert.IsFalse(list.Unify(con));
        }

        [Test]
        public void Unify_lis_eq_lis()
        {
            ListTerm list1 = new ListTerm();
            list1.Next = new ConstantTerm("ali");
            list1.Next.Next = new ConstantTerm("[]");

            ListTerm list2 = new ListTerm();
            list2.Next = new ConstantTerm("ali");
            list2.Next.Next = new ConstantTerm("[]");

            Assert.IsTrue(list1.Unify(list2));
        }

        [Test]
        public void Unify_lis_ne_lis()
        {
            ListTerm list1 = new ListTerm();
            list1.Next = new ConstantTerm("ali");
            list1.Next.Next = new ConstantTerm("[]");

            ListTerm list2 = new ListTerm();
            list2.Next = new ConstantTerm("ali");
            list2.Next.Next = new ConstantTerm("foo");

            Assert.IsFalse(list1.Unify(list2));
        }

        [Test]
        public void Unify_list_ref_lis()
        {
            ListTerm list1 = new ListTerm();
            list1.Next = new AbstractTerm();
            list1.Next.Next = new ConstantTerm("[]");

            ListTerm list2 = new ListTerm();
            list2.Next = new ConstantTerm("ali");
            list2.Next.Next = new ConstantTerm("[]");

            Assert.IsTrue(list1.Unify(list2));

            Assert.AreEqual(list1.Head.Data(), list2.Head.Data());
        }       


    }
}