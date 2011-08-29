using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class ObjectTerm : AbstractTerm
    {
        private object _data;


        public ObjectTerm()
        {
        }

        public ObjectTerm(object data)
        {
            _data = data;
        }

        public override string ToString()
        {
            return _data.ToString();
        }

        public override void Bind(AbstractTerm term)
        {

        }

        public override bool Unify(AbstractTerm term)
        {
            // Unify with a .NET object
            if (term.IsObject)
            {
                if (_data.Equals(term.Data()))
                {
                    return true;
                }
                else if (_data == term.Data())
                {
                    return true;
                }
                return false;
            }

            // Unify with a constant
            if (term.IsConstant)
            {
                // unify with an int
                int v;
                float x;
                if (Int32.TryParse((string)term.Data(), out v))
                {
                    
                    if (_data.GetType().ToString() == "System.Int32")
                    {
                        if (((int)_data) == v)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                // Unify with a float
                else if (float.TryParse((string)term.Data(), out x))
                {
                    if (_data.GetType().ToString() == "System.Float")
                    {
                        if (((float)_data) == x)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                // Unify with a constant atom/string
                else
                {
                    if (_data.GetType().ToString() == "System.String")
                    {
                        if (((string)_data) == ((string)term.Data()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }

            }

            // Unify with a list (need to unify it with an ArrayList)
            else
            {
                return false;
            }

            // Unify with a structure

            // Unify with a reference
        }

        public override object Data()
        {
            return _data;
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
            get { return false; }
        }

        public override bool IsObject
        {
            get { return true; }
        }

        public override bool IsStructure
        {
            get { return false; }
        }


    }
}
