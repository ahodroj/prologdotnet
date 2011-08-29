using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PredicateNameAttribute : Attribute
    {
        public PredicateNameAttribute(string name)
        {
            _name = name;
        }

        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
