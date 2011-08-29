using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class HeapNode
    {
        protected HeapNode _next;
        public HeapNode Next
        {
            get { return _next; }
            set { _next = value; }
        }

        protected HeapNode _previous;
        public HeapNode Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

    }
}
