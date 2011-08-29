using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime.PluginHost
{
    public class ExternalPredicate
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
