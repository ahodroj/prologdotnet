using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    abstract public class AbstractMachineFactory
    {
        abstract public AbstractDataArea CreateDataArea();
        abstract public AbstractProgram CreateProgram();
        abstract public AbstractTrail CreateTrail();
        abstract public AbstractAssemblyCache CreateCache();
    }
}
