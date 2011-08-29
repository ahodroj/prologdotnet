using System;
using System.Collections;
using System.Text;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime
{
    public class AMInstructionSet
    {
        private Hashtable _instructions = new Hashtable();

        public AMInstructionSet()
        {
            _instructions["allocate"] = new AllocateInstruction();
            _instructions["bcall"] = new BCallInstruction();
            _instructions["call"] = new CallInstruction();
            _instructions["cut"] = new CutInstruction();
            _instructions["deallocate"] = new DeallocateInstruction();
            _instructions["execute"] = new ExecuteInstruction();
            _instructions["fcall"] = new FCallInstruction();
            _instructions["fail"] = new FailInstruction();
            _instructions["get_constant"] = new GetConstantInstruction();
            _instructions["get_list"] = new GetListInstruction();
            _instructions["get_structure"] = new GetStructureInstruction();
            _instructions["get_value"] = new GetValueInstruction();
            _instructions["get_variable"] = new GetVariableInstruction();
            _instructions["halt"] = new HaltInstruction();
            _instructions["nop"] = new NopInstruction();
            _instructions["proceed"] = new ProceedInstruction();
            _instructions["put_constant"] = new PutConstantInstruction();
            _instructions["put_list"] = new PutListInstruction();
            _instructions["put_structure"] = new PutStructureInstruction();
            _instructions["put_unsafe_value"] = new PutUnsafeValueInstruction();
            _instructions["put_variable"] = new PutVariableInstruction();
            _instructions["put_value"] = new PutValueInstruction();
            _instructions["retry_me_else"] = new RetryMeElseInstruction();
            _instructions["set_constant"] = new SetConstantInstruction();
            _instructions["set_local_value"] = new SetLocalValueInstruction();
            _instructions["set_value"] = new SetValueInstruction();
            _instructions["set_variable"] = new SetVariableInstruction();
            _instructions["set_void"] = new SetVoidInstruction();
            _instructions["trust_me"] = new TrustMeInstruction();
            _instructions["try_me_else"] = new TryMeElseInstruction();
            _instructions["unify_constant"] = new UnifyConstantInstruction();
            _instructions["unify_local_value"] = new UnifyLocalValueInstruction();
            _instructions["unify_variable"] = new UnifyVariableInstruction();
            _instructions["unify_value"] = new UnifyValueInstruction();
            _instructions["unify_void"] = new UnifyVoidInstruction();
            _instructions["procedure"] = new ProcedureInstruction();
        }

        public bool IsValidInstruction(string instructionName)
        {
            return _instructions.ContainsKey(instructionName);
        }

        // deprecated method
        public bool isValidInstruction(string instructionName)
        {
            return _instructions.ContainsKey(instructionName);
        }

        public AbstractInstruction CreateInstruction(string instructionName, string arg1, string arg2, string arg3, string arg4)
        {
            string[] arguments = { arg1, arg2, arg3, arg4 };
            AbstractInstruction a = (AbstractInstruction)_instructions[instructionName];
            AbstractInstruction abstractInstruction = (AbstractInstruction)Activator.CreateInstance(a.GetType());
            abstractInstruction.Process(arguments);

            return abstractInstruction;
        }

        public AbstractInstruction CreateInstruction(string instructionName, string arg1, string arg2, string arg3)
        {
            return CreateInstruction(instructionName, arg1, arg2, arg3, "");
        }

        public AbstractInstruction CreateInstruction(string instructionName, string arg1, string arg2)
        {
            return CreateInstruction(instructionName, arg1, arg2, "", "");
        }

        public AbstractInstruction CreateInstruction(string instructionName, string arg1)
        {
            return CreateInstruction(instructionName, arg1, "", "", "");
        }

        public AbstractInstruction CreateInstruction(string instructionName)
        {
            return CreateInstruction(instructionName, "", "", "", "");
        }
        
    }
}
