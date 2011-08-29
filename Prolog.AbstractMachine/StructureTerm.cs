using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
    public class StructureTerm : AbstractTerm
    {

        private string _name;
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _arity;
        public override int Arity
        {
            get { return _arity; }
            set { _arity = value; }
        }


        
        public StructureTerm()
        {
            
        }

        public StructureTerm(string name, int arity)
        {
            _name = name;
            _arity = arity;
        }

        public override bool Unify(AbstractTerm term)
        {
            if (term.IsReference)
            {
                term.Assign(this);
                return true;
            }
            if (term.IsStructure)
            {
                if (term.Name != _name || term.Arity != _arity)
                {
                    return false;
                }

                for (int i = 0; i < _arity; i++)
                {
                    if (!this[i].Unify(term[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        public override void Bind(AbstractTerm term)
        {
            // do nothing
        }

        public override object Data()
        {
            return _name.Replace("'", "");
        }

        public override AbstractTerm Dereference()
        {
            return this;
        }

        public override AbstractTerm Reference()
        {
            return this;
        }

        public override AbstractTerm this[int index]
        {
            get { return GetArgumentIndex(index); }
        }

        private AbstractTerm GetArgumentIndex(int index)
        {
            AbstractTerm term = this;

            if (index == 0)
                return (AbstractTerm)_next ;

            for (int i = 0; i <= index; i++)
            {
                term = (AbstractTerm)term.Next;
            }

            return term;
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
            get { return false; }
        }

        public override bool IsObject
        {
            get { return false; }
        }

        public override bool IsStructure
        {
            get { return true; }
        }

        public override string ToString()
        {
            string str = _name + "(";
            for (int i = 0; i < _arity; i++)
            {
                AbstractTerm term = (AbstractTerm)this[i];
                str += term.ToString();
                if (i == _arity - 1)
                {
                    str += ")";
                }
                else
                {
                    str += ",";
                }
            }
            return str;
        }
    }
}
