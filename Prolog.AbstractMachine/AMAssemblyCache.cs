using System;
using System.Collections;
using System.IO;
using System.Reflection;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime
{
    /// <summary>
    /// Summary description for WAMProgram.
    /// </summary>
    public class AMAssemblyCache : AbstractAssemblyCache
    {
        private Hashtable _localAssemblyCache;
        public Hashtable LocalAssemblyCache
        {
            get { return _localAssemblyCache; }
            set { _localAssemblyCache = value; }
        }

        private ArrayList _assemblyFiles;
        public ArrayList AssemblyFiles
        {
            get { return _assemblyFiles; }
            set { _assemblyFiles = value; }
        }

        private ArrayList _namespaces;
        public ArrayList Namespaces
        {
            get { return _namespaces; }
            set { _namespaces = value; }
        }

        public AMAssemblyCache()
            : base()
        {
            _localAssemblyCache = new Hashtable();
            _assemblyFiles = new ArrayList();
            _namespaces = new ArrayList();

        }
        public override void Init()
        {
            // Initialize global assembly cache
            InitializeLocalAssemblyCache();
        }

        public override void LoadAssemblyFiles(ArrayList a)
        {
            _assemblyFiles.AddRange(a);
        }

        public override void LoadNamespaces(ArrayList n)
        {
            _namespaces.AddRange(n);
        }

        public override void Load(ArrayList namespaces, ArrayList assemblyFiles)
        {
            if (_namespaces != null && _namespaces.Count > 0)
            {
                _namespaces.AddRange(namespaces);
            }
            _assemblyFiles.AddRange(assemblyFiles);
        }

        private void InitializeLocalAssemblyCache()
        {
            Assembly a = null;
            string[] bcl = { 
                "System", 
                "System.Data", 
                "System.EnterpriseServices",
                "mscorlib",
                "System.Design",
                "System.DirectoryServices",
                "System.XML",
                "System.Windows.Forms"
            };

            foreach (string asm in bcl)
            {
                a = Assembly.LoadWithPartialName(asm);
                foreach (Type t in a.GetExportedTypes())
                {
                    _localAssemblyCache.Add(t.ToString(), asm);
                }
            }
        }
    }
}