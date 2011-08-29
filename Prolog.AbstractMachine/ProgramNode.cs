using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Runtime
{
    public class ProgramNode
    {
        protected ProgramNode _next;
        public ProgramNode Next
        {
            get { return _next; }
            set { _next = value; }
        }

        protected AbstractInstruction _instruction;
        public AbstractInstruction Instruction
        {
            get { return _instruction; }
            set { _instruction = value; }
        }

        public ProgramNode()
        {
            _instruction = null;
            _next = null;
        }

        public ProgramNode(AbstractInstruction instruction)
        {
            _instruction = instruction;
            _next = null;
        }

        public ProgramNode(AbstractInstruction instruction, ProgramNode next)
        {
            _instruction = instruction;
            _next = next;
        }
    }
}
