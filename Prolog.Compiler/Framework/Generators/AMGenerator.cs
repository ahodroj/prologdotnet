using System;
using System.Collections.Generic;
using System.Text;
using Axiom.Runtime;
using System.Collections;

namespace Axiom.Compiler.Framework.Generators
{
    public sealed class AMGenerator
    {
        private AMInstructionSet _instructionSet = new AMInstructionSet();
        private ArrayList _instructions = new ArrayList();

        public AMGenerator(ArrayList instructions)
        {
            _instructions = instructions;
        }

        public void Emit(int opcode, string arg1, string arg2, string arg3, string arg4)
        {
            _instructions.Add(_instructionSet.CreateInstruction(OpCodes.OpcodeStrings[opcode], arg1, arg2, arg3, arg4));
        }

        public void Emit(int opcode, string arg1, string arg2, string arg3)
        {
            _instructions.Add(_instructionSet.CreateInstruction(OpCodes.OpcodeStrings[opcode], arg1, arg2, arg3, ""));
        }

        public void Emit(int opcode, string arg1, string arg2)
        {
            _instructions.Add(_instructionSet.CreateInstruction(OpCodes.OpcodeStrings[opcode], arg1, arg2, "", ""));
        }

        public void Emit(int opcode, string arg1)
        {
            _instructions.Add(_instructionSet.CreateInstruction(OpCodes.OpcodeStrings[opcode], arg1, "", ""));
        }
        /// <summary>
        /// Generates an abstract machine instruction.
        /// </summary>
        /// <param name="opcode">instruction opcode.</param>
        /// <returns>AbstractInstruction object</returns>
        public void Emit(int opcode)
        {
            _instructions.Add(_instructionSet.CreateInstruction(OpCodes.OpcodeStrings[opcode], ""));
        }

        public void EmitCall(string name, int arity)
        {
            _instructions.Add(_instructionSet.CreateInstruction("call", name, arity.ToString()));
        }

        public void EmitCallVar(string name, int arity)
        {
            _instructions.Add(_instructionSet.CreateInstruction("callvar", name, arity.ToString()));
        }

        public void EmitFCall(string procName, string methodName, string assembly, string classType)
        {
            _instructions.Add(_instructionSet.CreateInstruction("fcall", assembly, classType, methodName, procName));
        }

        public void EmitBCall(IAbstractMachinePredicate pred)
        {
            _instructions.Add(_instructionSet.CreateInstruction("bcall", pred.Name() + "/" + pred.Arity()));

        }

        public void EmitExecute(string name, int arity)
        {
            _instructions.Add(_instructionSet.CreateInstruction("execute", name, arity.ToString()));
        }

        public void EmitExecuteVar(string name, int arity)
        {
            _instructions.Add(_instructionSet.CreateInstruction("executevar", name, arity.ToString()));
        }


        public void DeclareProcedure(string name, int arity)
        {
            _instructions.Add(_instructionSet.CreateInstruction("procedure", name, arity.ToString()));
        }

        public void EndProcedure()
        {
            _instructions.Add(_instructionSet.CreateInstruction("proceed"));
        }

        public ArrayList Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }
    }
}
