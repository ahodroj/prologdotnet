using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    abstract public class AbstractInstruction
    {
        public object[] _arguments;
        
        abstract public string Name();
        abstract public int NumberOfArguments();
        abstract public void Execute(AbstractMachineState state);
        abstract public void Process(object[] arguments);
    }
}
