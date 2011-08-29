using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    abstract public class AbstractTrail
    {
        abstract public void Initialize();
        abstract public bool Stop();
        abstract public void Trail(AbstractTerm term);
    }
}
