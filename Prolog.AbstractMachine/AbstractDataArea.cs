using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
    abstract public class AbstractDataArea
    {
        abstract public void Initialize(AbstractMachineState state);
        abstract public bool Stop();
    }
}
