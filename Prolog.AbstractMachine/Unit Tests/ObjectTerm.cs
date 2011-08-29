

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _ObjectTerm
    {

        [Test]
        public void IsReference()
        {
            ObjectTerm t = new ObjectTerm();
            AbstractTerm a = new ObjectTerm();
            Assert.IsFalse(t.IsReference);
            Assert.IsFalse(a.IsReference);

        }

        [Test]
        public void IsConstant()
        {
            ObjectTerm t = new ObjectTerm();
            AbstractTerm a = new ObjectTerm();
            Assert.IsFalse(t.IsConstant);
            Assert.IsFalse(a.IsConstant);

        }

        [Test]
        public void IsStructure()
        {
            ObjectTerm t = new ObjectTerm();
            AbstractTerm a = new ObjectTerm();
            Assert.IsFalse(t.IsStructure);
            Assert.IsFalse(a.IsStructure);
        }

        [Test]
        public void IsList()
        {
            ObjectTerm t = new ObjectTerm();
            AbstractTerm a = new ObjectTerm();
            Assert.IsFalse(t.IsList);
            Assert.IsFalse(a.IsList);
        }

        [Test]
        public void IsObject()
        {
            ObjectTerm t = new ObjectTerm();
            AbstractTerm a = new ObjectTerm();
            Assert.IsTrue(t.IsObject);
            Assert.IsTrue(a.IsObject);

        }

        [Test]
        public void ObjectTerm()
        {
            ObjectTerm ot = new ObjectTerm(33);

            Assert.AreEqual(33, ot.Data());
        }

        

        [Test]
        public void Data()
        {
            ObjectTerm t = new ObjectTerm();
            Assert.IsNull(t.Data());
        }



        [Test]
        public void Bind()
        {
            //ObjectTerm term = new ObjectTerm();
            //ObjectTerm term2 = new ObjectTerm();

            //term.Bind(term2);

            //Assert.AreSame(term, term.Reference());

        }

       

        [Test]
        public void Dereference()
        {
            //ObjectTerm term1 = new ObjectTerm();
            //ObjectTerm term2 = new ObjectTerm();
            //ObjectTerm term3 = new ObjectTerm();

            //term2.Bind(term3);
            //term1.Bind(term2);

            //Assert.AreSame(term1, term1.Dereference());
        }

        [Test]
        public void Reference()
        {
            //ObjectTerm term1 = new ObjectTerm();
            //ObjectTerm term2 = new ObjectTerm();

            //term1.Bind(term2);

            //Assert.AreSame(term1, term1.Reference());
        }


    }
}