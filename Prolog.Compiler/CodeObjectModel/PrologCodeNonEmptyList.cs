//------------------------------------------------------------------------------
// <copyright file="PrologCodeNonEmptyList.cs" company="Axiom">
//     
//      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
//     
//      The use and distribution terms for this source code are contained in the file
//      named license.txt, which can be found in the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by the
//      terms of this license.
//     
//      You must not remove this notice, or any other, from this software.
//     
// </copyright>                                                                
//------------------------------------------------------------------------------



using System;

namespace Axiom.Compiler.CodeObjectModel
{	
	/// <summary>
	/// Represents a non-empty Prolog list.
	/// </summary>
	public class PrologCodeNonEmptyList
	:	PrologCodeList
	{
		private PrologCodeTerm _head;
		private PrologCodeTerm _tail;
		
		public PrologCodeNonEmptyList (PrologCodeTerm head, PrologCodeTerm tail)
		{
			this._head = head;
			this._tail = tail;
		}
		
		public PrologCodeNonEmptyList (PrologCodeTerm head)
		{
			this._head = head;
			this._tail = null;
		}
		
		public override void Append (PrologCodeTerm item)
		{
			
		}
		
		public PrologCodeTerm Head
		{
			get
			{
				return this._head;
			}
		}
		
		public PrologCodeTerm Tail
		{
            get { return this._tail; }
            set { _tail = value; }
		}

        public override string ToString()
        {
            string listStr = "[";
            listStr += _head.ToString();
            if (_tail is PrologCodeNonEmptyList)
            {
                listStr += GetListTailStr((PrologCodeNonEmptyList)_tail) + "]";
            }
            else if (_tail is PrologCodeEmptyList)
            {
                listStr += "]";
            }
            else
            {
                listStr += "|" + _tail.ToString() + "]";
            }

            return listStr;
        }

        private string GetListTailStr(PrologCodeNonEmptyList list)
        {
            string listStr = "";
            if (list.Head is PrologCodeNonEmptyList)
            {
               // listStr += "[" + GetListTailStr((PrologCodeNonEmptyList)list.Head) + "]";
                listStr += "," + list.Head.ToString();
            }
            else
            {
                listStr += "," + list.Head;
            }

            if (list.Tail is PrologCodeNonEmptyList)
            {
                listStr += GetListTailStr((PrologCodeNonEmptyList)list.Tail);
            }
            else if (list.Tail is PrologCodeEmptyList)
            {

            }
            else
            {
                listStr += "|" + list.Tail.ToString();
            }

            return listStr;

        }
	}
}