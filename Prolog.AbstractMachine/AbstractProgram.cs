using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
	abstract public class AbstractProgram
	{
        abstract public void Initialize(ArrayList program);
        abstract public bool Stop();
        abstract public AbstractInstruction CurrentInstruction();
	}
}

