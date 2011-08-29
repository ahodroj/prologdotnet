using System;
using System.Collections;
using System.Text;
using System.Reflection;
using Axiom.Runtime;

namespace Axiom.Runtime.Builtins.OOP
{
    
    public class get_property_3 : AbstractPredicate
    {
        private AbstractMachineState runtime = null;
        private string fullTypeName = "";

        public override void Execute(AbstractMachineState state)
        {
            AMProgram prog = (AMProgram)state.Program;

            runtime = state;
            // invoke(+ClassObj,+meth(+x..),-Return).
            AbstractTerm X0 = (state["X0"] as AbstractTerm).Dereference();
            AbstractTerm X1 = (state["X1"] as AbstractTerm).Dereference();
            AbstractTerm X2 = (state["X2"] as AbstractTerm).Dereference();
            
            if (Getproperty(X0, X1, X2))
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


        public bool Getproperty(AbstractTerm obj, AbstractTerm method, AbstractTerm returnObject)
        {
            if (obj.IsConstant)
            {
                // invoke a static get property
                Assembly asm = GetRequiredAssembly(obj.Data() as string, runtime); ;
                Type type = asm.GetType(obj.Data() as string);
                if (type == null)
                {
                    return false;
                }
                if (type.GetProperty(method.Data() as string) == null)
                {
                    return false;
                }
                object res = type.InvokeMember(method.Data() as string, BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public, null, null, null);
                switch (res.GetType().ToString())
                {
                    case "System.String":
                        AbstractTerm rC = new ConstantTerm(res.ToString());
                        if (!returnObject.Unify(rC))
                        {
                            return false;
                        }
                        break;
                    case "System.Char":
                    case "System.Int32":
                    case "System.Boolean":
                        AbstractTerm returnConstant = new ConstantTerm(res.ToString());
                        if (!returnObject.Unify(returnConstant))
                        {
                            return false;
                        }
                        break;
                    default:
                        returnObject.Unify(new ObjectTerm(res));
                        break;
                }
            }
            else
            {
                if (obj.Data().GetType().GetProperty(method.Data() as string) == null)
                {
                    return false;
                }

                // invoke an instance get property
                object res = obj.Data().GetType().InvokeMember(method.Data() as string, BindingFlags.Default | BindingFlags.ExactBinding | BindingFlags.GetProperty, null, obj.Data(), null);
                switch (res.GetType().ToString())
                {
                    case "System.Char":
                    case "System.String":
                    case "System.Int32":
                    case "System.Boolean":
                        returnObject.Unify(new ConstantTerm(res.ToString()));
                        break;
                    default:
                        returnObject.Unify(new ObjectTerm(res));
                        break;
                }
            }
            return true;
        }

        public override int Arity()
        {
            return 3;
        }

        public override string Name()
        {
            return "get_property";
        }
    }
}
