using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class AMFactory : AbstractMachineFactory
    {
        public AMFactory()
        {
        }

        public override AbstractDataArea CreateDataArea()
        {
            return new AMHeap();
        }

        public override AbstractProgram CreateProgram()
        {
            return new AMProgram();
        }

        public override AbstractTrail CreateTrail()
        {
            return AMTrail.Instance;
        }

        public override AbstractAssemblyCache CreateCache()
        {
            return new AMAssemblyCache();
        }
    }
}

