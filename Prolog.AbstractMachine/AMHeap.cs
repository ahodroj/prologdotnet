using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class AMHeap : AbstractDataArea
    {
        private int _variableIndex = 0;

        private AbstractMachineState _state;
        
        private HeapNode _extraItem;

        private HeapNode _heap;

        private HeapNode _previousItem;
        
        private HeapNode _h;
        public HeapNode H
        {
            get { return _h; }
            set { _h = value; }
        }

        public AMHeap()
        {
            
        }

        public void Push(HeapNode newItem)
        {
            if (newItem is AbstractTerm)
            {
                AbstractTerm at = newItem as AbstractTerm;
                if (at.IsReference)
                {
                    if (at.Name == null || at.Name == "")
                    {
                        at.Name = "__" + _variableIndex.ToString();
                        _variableIndex++;
                    }
                }
            }
            bool changeS = (_state.S == _extraItem); 
            if (_h == null)
            {
                _heap = newItem;
                _extraItem = new HeapNode();
                newItem.Next = _extraItem;
                _extraItem.Previous = newItem;
                _h = newItem;
                return;
            }
       
            
            _extraItem = newItem;
            _extraItem.Previous = _h;
            _h.Next = _extraItem;
            _h = _h.Next;
            if (changeS)
            {
                _state.S = _h;
            }

            HeapNode e = new HeapNode();
            _extraItem.Next = e;
            e.Previous = _extraItem;
            _extraItem = _extraItem.Next;

        }

        //public void Push(HeapNode newItem)
        //{
        //    if (_h == null)
        //    {
        //        _heap = newItem;
        //        _h = newItem;
        //        return;
        //    }
        //    _h.Next = newItem;
        //    _previousItem = _h;
        //    _h = _h.Next;
        //    _h.Previous = _previousItem;
        //}

        public object Pop()
        {
            // if heap is empty, return null
            if (_h == null)
            {
                return null;
            }
            // top of the heap
            HeapNode topOfHeap = _h;
            
            // if the heap doesn't have one item only, go backwards
            if (_h.Previous != null)
            {
                _h = _h.Previous;
                _h.Next = _extraItem;
                _extraItem.Previous = _h;
            }
            else
            {
                _h = null;
            }

            return topOfHeap;
        }

        public object Top()
        {
            return _h;
        }

        public override void Initialize(AbstractMachineState state)
        {
            _state = state;
        }

        public override bool Stop()
        {
            return true;
        }

        public override string ToString()
        {
            string str = "";
            for (HeapNode h = _heap; h != null; h = h.Next)
            {
                str += " " + h.ToString() + " ";
            }
            return str;
        }
    }
}
