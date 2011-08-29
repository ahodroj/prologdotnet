using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class ListTerm : AbstractTerm
    {
        public override AbstractTerm Head
        {
            get {
               return (AbstractTerm)base.Next;
            }
        }

        public override AbstractTerm Tail
        {
            get { return (AbstractTerm)base.Next.Next; }
        }

        public ListTerm()
        {
        }

        public override void Bind(AbstractTerm term)
        {
            // do nothing
        }

        public override bool Unify(AbstractTerm term)
        {
            if (term.IsReference)
            {
                term.Assign(this);
                return true;
            }
            if (term.IsList)
            {
                return Head.Unify(term.Head) && Tail.Unify(term.Tail);
            }

            return false;

        }
        public override object Data()
        {
            return null;
        }

        public override AbstractTerm Dereference()
        {
            return this;
        }

        public override AbstractTerm Reference()
        {
            return this;
        }

        public override bool IsReference
        {
            get { return false; }
        }

        public override bool IsConstant
        {
            get { return false; }
        }

        public override bool IsList
        {
            get { return true; }
        }

        public override bool IsObject
        {
            get { return false; }
        }

        public override bool IsStructure
        {
            get { return false; }
        }

        public override string ToString()
        {
       
            string str = Head.ToString();
            if (str == "[]")
                str = "";
            
            if (Tail != null)
            {
                
                    str += "," + Tail.ToString();
                
               
            }
            else
            {
                
            }

            return str;
        }

    }
}
