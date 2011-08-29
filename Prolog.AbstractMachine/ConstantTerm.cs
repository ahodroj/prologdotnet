using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class ConstantTerm : AbstractTerm
    {
        private object _data;
        

        public ConstantTerm(object constant)
        {
            _data = constant;
        }

        public ConstantTerm()
        {
        }

        public override void Bind(AbstractTerm term)
        {
           
        }

        public override bool Unify(AbstractTerm term)
        {

            if (term.IsReference)
            {
                term.Assign(this);
                return true;
            }
            if (term.IsConstant)
            {
                bool res = term.Data().Equals(this.Data());
                return res;
            }

            return false;
        }

        public override object Data()
        {
            //// New fix
            // if this is a string, then return without a ' '
            if (_data is string)
            {
                string data = _data as string;
                if (data.IndexOf('"') != -1)
                {
                    string value = data.Trim(new char[] { '"' });
                    int integer;
                    if (Int32.TryParse(value, out integer))
                    {
                        return data;
                    }
                    else
                    {
                        return value;
                    }
                }
                else if (data.IndexOf('\'') != -1)
                {
                    string value = data.Trim(new char[] { '\'' });
                    int integer;
                    if (Int32.TryParse(value, out integer))
                    {
                        return data;
                    }
                    else
                    {
                        return value;
                    }
                }
            }
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
            get { return true; }
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
            get { return false; }
        }

        public override string ToString()
        {
            return Data().ToString();
        }
    }
}
