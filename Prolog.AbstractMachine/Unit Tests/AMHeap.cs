

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AMHeap
    {

        [Test]
        public void Initialize()
        {
            AMHeap heap = new AMHeap();
            AbstractMachineState state = new AbstractMachineState(new AMFactory());

            heap.Initialize(state);

            Assert.IsNull(heap.H);
            Assert.IsNull(heap.Top());
           
        }

        [Test]
        public void Stop()
        {
            AMHeap heap = new AMHeap();

            Assert.IsTrue(heap.Stop());
        }

        [Test]
        public void Push_AbstractTerm()
        {
            AMHeap heap = new AMHeap();

            AbstractTerm term = new AbstractTerm();
            heap.Push(term);

            Assert.AreSame(heap.Top(), term);
        }

        [Test]
        public void Push_ConstantTerm()
        {
            AMHeap heap = new AMHeap();

            ConstantTerm con = new ConstantTerm("Hello, World!");

            heap.Push(con);

            Assert.AreSame(con, heap.Top());
        }

        [Test]
        public void Push_StructureTerm()
        {
            AMHeap heap = new AMHeap();

            StructureTerm con = new StructureTerm("Hello, World!", 2);

            heap.Push(con);

            Assert.AreSame(con, heap.Top());
        }

        [Test]
        public void Push_ListTerm()
        {
            AMHeap heap = new AMHeap();

            ListTerm con = new ListTerm();

            heap.Push(con);

            Assert.AreSame(con, heap.Top());
        }

        [Test]
        public void Push_ObjectTerm()
        {
            AMHeap heap = new AMHeap();

            ObjectTerm con = new ObjectTerm();

            heap.Push(con);

            Assert.AreSame(con, heap.Top());
        }


        [Test]
        public void Pop_one_item()
        {
            AMHeap heap = new AMHeap();

            ConstantTerm con = new ConstantTerm("ali");

            heap.Push(con);

            heap.Pop();

            Assert.IsNull(heap.Top());
        }

        [Test]
        public void Pop_two_items()
        {
            AMHeap heap = new AMHeap();

            ConstantTerm con = new ConstantTerm("ali");
            ConstantTerm first = new ConstantTerm("foo");

            heap.Push(first);
            heap.Push(con);

            heap.Pop();

            Assert.AreSame(first, heap.Top());
        }

        [Test]
        public void Pop_an_empty_heap()
        {
            AMHeap heap = new AMHeap();

            object top = heap.Pop();

            Assert.IsNull(top);
        }



  
    }
}