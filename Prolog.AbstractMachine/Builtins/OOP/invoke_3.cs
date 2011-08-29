using System;
using System.Collections;
using System.Text;
using System.Reflection;
using Axiom.Runtime;


namespace Axiom.Runtime.Builtins.OOP
{
    // invoke(O, M, R).
    public class invoke_2 : AbstractPredicate
    {
        private AbstractMachineState runtime = null;
        private string fullTypeName = "";

        public override void Execute(AbstractMachineState state)
        {
            AMProgram prog = (AMProgram)state.Program;


            runtime = state;
            // invoke(+ClassObj,+meth(+x..),-Return).
            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            AbstractTerm X1 = ((AbstractTerm)state["X1"]).Dereference();
            AbstractTerm X2 = ((AbstractTerm)state["X2"]).Dereference();

            
            if (InvokeMethod(X0, X1, X2, state))
            {
                prog.Next();
            }
            else
            {
                state.Backtrack();
            }
        }

        private Assembly GetRequiredAssembly(string classType, AbstractMachineState state)
        {
            AMAssemblyCache cache = (AMAssemblyCache)state.AssemblyCache;
            string assemblyFile = "";

            // if it exists in the global assembly cache, then get it from there...
            foreach (string ns in cache.Namespaces)
            {
                string fullName = ns + "." + classType;
                if (cache.LocalAssemblyCache.Contains(fullName))
                {
                    assemblyFile = (string)cache.LocalAssemblyCache[fullName];
                    return Assembly.LoadWithPartialName(assemblyFile);
                }
            }

            foreach (string loadedAssembly in cache.AssemblyFiles)
            {
                Assembly a = Assembly.LoadFrom(loadedAssembly);

                Type[] types = a.GetTypes();
                Type type = null;
                foreach (Type t in types)
                {
                    if (t.Name == classType)
                    {
                        type = t;
                        break;
                    }
                }

                fullTypeName = type.FullName;
                return a;
            }
            return null;
        }


        private bool InvokeMethod(AbstractTerm obj, AbstractTerm method, AbstractTerm returnObject, AbstractMachineState state)
        {
            
            if (obj.IsConstant)
            {
                // invoke a static method
                Assembly asm = GetRequiredAssembly(obj.Data() as string, state);
                Type type = asm.GetType(obj.Data() as string);
                if (type == null)
                {
                    return false;
                }
                ArrayList paramArray = new ArrayList();
                GetTypes(type.GetMethod(method.Data() as string), method, ref paramArray, obj);

                object[] arguments = paramArray.ToArray();
                object res = type.InvokeMember(method.Data() as string, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, arguments);
                switch (res.GetType().ToString())
                {
                    case "System.Char":
                    case "System.String":
                    case "System.Int32":
                    case "System.Boolean":
                        returnObject.Assign(new ConstantTerm(res.ToString()));
                        break;
                    default:
                        returnObject.Assign(new ObjectTerm(res));
                        break;
                }
                SetTypes(method, arguments);
            }
            else
            {
                // invoke an instance method
                ArrayList paramArray = new ArrayList();

                GetTypes(obj.Data().GetType().GetMethod(method.Data() as string), method, ref paramArray, obj);
                object classObject = obj.Data();
                MethodInfo omi = classObject.GetType().GetMethod(method.Data() as string);

                object[] arguments = paramArray.ToArray();
                object res = obj.Data().GetType().InvokeMember(method.Data() as string, BindingFlags.Default | BindingFlags.ExactBinding | BindingFlags.InvokeMethod, null, obj.Data(), arguments);
                if (res != null)
                {
                    switch (res.GetType().ToString())
                    {

                        case "System.String":
                            ConstantTerm rC = new ConstantTerm(res.ToString());
                            if (!returnObject.Unify(rC))
                            {
                                return false;
                            }
                            break;
                        case "System.Char":
                        case "System.Int32":
                        case "System.Boolean":
                            ConstantTerm returnConstant = new ConstantTerm(res.ToString());
                             if (!returnObject.Unify(returnConstant))
                            {
                                return false;
                            }
                            break;
                        default:
                            returnObject.Assign(new ObjectTerm(res));
                            break;
                    }
                }
                SetTypes(method, arguments);
            }
            return true;
        }

        // Get types and set types
        private void GetTypes(MethodInfo mi, AbstractTerm methodStructObj, ref ArrayList paramArray, AbstractTerm obj)
        {
            AMHeap da = (AMHeap)runtime.DataArea;
            AbstractTerm methodObj = (AbstractTerm)methodStructObj.Next;
            
            ParameterInfo[] parms = mi.GetParameters();
            for (int i = 0; i < parms.Length; i++)
            {
                ParameterInfo par = (ParameterInfo)parms[i];
                AbstractTerm var = (AbstractTerm)methodObj;
                if (i != parms.Length - 1)
                {
                    methodObj = (AbstractTerm)methodObj.Next;
                }
                switch (par.ParameterType.ToString())
                {
                    case "System.Int32":
                        paramArray.Add(Int32.Parse(var.Data() as string));
                        break;
                    case "System.Char":
                        paramArray.Add(((string)var.Data())[0]);
                        break;
                    case "System.String":
                        paramArray.Add(var.Data() as string);
                        break;
                    case "System.Boolean":
                        paramArray.Add(Boolean.Parse(var.Data() as string));
                        break;
                    default:	// pass Variable.Object
                        paramArray.Add(var);
                        break;
                }
            }
        }

        private void SetTypes(AbstractTerm obj, object[] arguments)
        {
            AMHeap da = (AMHeap)runtime.DataArea;
            AbstractTerm argObj = obj;

            for (int i = 0; i < arguments.Length; i++)
            {
                AbstractTerm a = (AbstractTerm)argObj.Next;
                argObj = (AbstractTerm)argObj.Next;

                if (!a.IsObject)
                {
                    // Need to check if unification is the apropriate method
                    a.Unify(new ConstantTerm(arguments[i].ToString()));
                }
                else
                {
                    // Need to check if unification is the apropriate method
                    a.Unify(new ObjectTerm(arguments[i]));
                }
            }
        }

        public override int Arity()
        {
            return 3;
        }

        public override string Name()
        {
            return "invoke";
        }
    }
}
