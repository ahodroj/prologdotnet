using System;
using System.Collections;

namespace Axiom.Runtime
{
    public abstract class AbstractAssemblyCache
    {
        public abstract void Init();

        public abstract void LoadNamespaces(ArrayList n);

        public abstract void LoadAssemblyFiles(ArrayList a);

        public abstract void Load(ArrayList namespaces, ArrayList assemblyFiles);
    }
}
