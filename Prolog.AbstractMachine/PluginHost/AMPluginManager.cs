using System;
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;

namespace Axiom.Runtime.PluginHost
{
    public class AMPluginManager
    {
        private ArrayList _plugins;
        public ArrayList Plugins
        {
            get { return _plugins; }
            set { _plugins = value; }
        }

        public AMPluginManager()
        {
            AMExtensions ext = new AMExtensions();

            _plugins = ext.Load();
        
        }

      

        
    }
}
