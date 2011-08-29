using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace Axiom.Runtime.Instructions
{
    public class FCallInstruction : AbstractInstruction
    {
        public string _predicateName;
        public string _assemblyName;
        public string _classType;
        public string _methodName;

        //
        // fcall <predicate name>,<assembly name>,<class type>,<method name>
        // fcall male,Prolog.dll,Class1,CreateLogic
        public override string Name()
        {
            return "fcall";
        }

        public override int NumberOfArguments()
        {
            return 4;
        }

        public override void Execute(AbstractMachineState state)
        {
            AMProgram program = (AMProgram)state.Program;
            AMHeap heap = (AMHeap)state.DataArea;

            ExecuteForeignPredicate(state);
        }

        private void ExecuteForeignPredicate(AbstractMachineState state)
        {
            Assembly loadedAssembly = Assembly.LoadFrom(_assemblyName);
            Type[] types = loadedAssembly.GetTypes();

            AMForeignPredicate fp = state.GetForeignPredicate(_predicateName);

            if (fp == null)
            {
                throw new Exception("INTERNAL ERROR: cannot find foreign predicate " + _predicateName);
            }

            foreach (Type type in types)
            {
                if (type.Name == _classType)
                {
                    foreach (MethodInfo mi in type.GetMethods())
                    {
                        if (mi.Name == _methodName)
                        {
                            if (mi.ReturnType == typeof(bool))
                            {
                                fp.ReturnType = AMForeignPredicate.R_BOOL;
                            }
                            else
                            {
                                fp.ReturnType = AMForeignPredicate.R_VOID;
                            }

                            bool foreignMethodResult = InvokeForeignPredicate(type, mi, fp, loadedAssembly, state);
                            // bool, fail = !foreignMethodResult (if fail, backtrack, else P++)
                            // internal, (if fail, backtrack, else P++)
                            // void, if fail backtrack, else P++

                            if (fp.ReturnType == AMForeignPredicate.R_BOOL)
                            {
                                state.Fail = !foreignMethodResult;
                            }
                            else if (fp.ReturnType == AMForeignPredicate.R_VOID)
                            {
                                ((AMProgram)state.Program).Next();
                                return;
                            }
                            if (state.Fail)
                            {
                                state.Backtrack();
                            }
                            else
                            {
                                ((AMProgram)state.Program).Next();
                            }
                        }
                    }
                }
            }
        }

        private bool InvokeForeignPredicate(Type type, MethodInfo methodInfo, AMForeignPredicate fp, Assembly loadedAssembly, AbstractMachineState state)
        {
            
            int registerIndex = 0;
            ArrayList methodArguments = new ArrayList();

            foreach (AMForeignPredicateArgument arg in fp.Arguments)
            {
                AbstractTerm term = ((AbstractTerm)state["X" + registerIndex.ToString()]).Dereference();
                switch (arg.Type)
                {
                    case AMForeignPredicateArgument.T_BOOL:
                        bool bResult = false;
                        if (!Boolean.TryParse(term.Data().ToString(), out bResult))
                        {
                            return false;
                        }
                        methodArguments.Add(System.Boolean.Parse(term.Data().ToString()));
                        break;
                    case AMForeignPredicateArgument.T_CHAR:
                        if (term.Data() == null)
                        {
                            return false;
                        }
                        methodArguments.Add(Convert.ToChar(term.Data()));
                        break;
                    case AMForeignPredicateArgument.T_DOUBLE:
                    case AMForeignPredicateArgument.T_FLOAT:
                    case AMForeignPredicateArgument.T_INTEGER:
                        int iResult = 0;
                        if (!Int32.TryParse(term.Data().ToString(), out iResult))
                        {
                            return false;
                        }
                        methodArguments.Add(Int32.Parse(term.Data().ToString()));
                        break;

                    case AMForeignPredicateArgument.T_STRING:
                        methodArguments.Add(term.Data().ToString());
                        break;
                    case AMForeignPredicateArgument.T_TERM:
                        methodArguments.Add(term);
                        break;
                }
                registerIndex++;
            }
            return InvokePredicate(type, methodInfo, fp, state, loadedAssembly, methodArguments);

        }

        private bool InvokePredicate(Type type, MethodInfo methodInfo, AMForeignPredicate fp, AbstractMachineState state, Assembly loadedAssembly, ArrayList methodArguments)
        {
            object[] arguments = methodArguments.ToArray();
            object returnedResult = null;
            object baseObject = null;

            baseObject = Activator.CreateInstance(type);
            returnedResult = type.InvokeMember(_methodName,
                                BindingFlags.Default | BindingFlags.InvokeMethod,
                                null,
                                baseObject,
                                arguments);
            int registerIndex = 0;
            foreach (AMForeignPredicateArgument fArgument in fp.Arguments)
            {
                if (fArgument.PassingType == AMForeignPredicateArgument.PASS_OUT ||
                   fArgument.PassingType == AMForeignPredicateArgument.PASS_INOUT)
                {
                    AbstractTerm term = ((AbstractTerm)state["X" + registerIndex.ToString()]).Dereference();
                    if (fArgument.Type == AMForeignPredicateArgument.T_TERM)
                    {
                        ConstantTerm argumentVariable = new ConstantTerm(arguments[registerIndex]);
                        term.Assign(argumentVariable);
                    }
                    else
                    {
                        if (fArgument.Type == AMForeignPredicateArgument.T_STRING)
                        {
                            if (term.IsReference)
                            {
                                term.Assign(new ConstantTerm(arguments[registerIndex]));
                            }
                        }
                    }
                    registerIndex++;
                }
            }
            bool returnValue = false;
            if (fp.ReturnType == AMForeignPredicate.R_BOOL)
            {
                if (returnedResult == null)
                {
                    return true;
                }
                else
                {
                    returnValue = (bool)returnedResult;
                }
            }

            return returnValue;
        }

        public override void Process(object[] arguments)
        {
            _arguments = arguments;
            _assemblyName = (string)arguments[0];
            _classType = (string)arguments[1];
            _methodName = (string)arguments[2];
            _predicateName = (string)arguments[3];
        }


        public override string ToString()
        {
            return Name() + "[" + _assemblyName + "]" + _classType + "::" + _methodName + ", " + _predicateName;
        }
    }
}
