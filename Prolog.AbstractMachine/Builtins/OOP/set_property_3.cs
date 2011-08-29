using System;
using System.Collections;
using System.Text;
using System.Reflection;
using Axiom.Runtime;

namespace Axiom.Runtime.Builtins.OOP
{
    public class set_property_3 : AbstractPredicate
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
           
            if (Setproperty(X0, X1, X2))
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



        public bool Setproperty(AbstractTerm obj, AbstractTerm method, AbstractTerm objValue)
        {
            if (obj.IsConstant)
            {
                // invoke a static method
                Assembly asm = GetRequiredAssembly(obj.Data() as string, runtime);
                Type type = asm.GetType(obj.Data() as string);
                if (type == null)
                {
                    return false;
                }

                ArrayList paramArray = new ArrayList();
                ParameterInfo par = type.GetMethod(method.Data() as string).GetParameters()[0];
                switch (par.GetType().ToString())
                {
                    case "System.Int32":
                        paramArray.Add(Int32.Parse(objValue.Data() as string));
                        break;
                    case "System.Char":
                        paramArray.Add(objValue.Data().ToString()[0]);
                        break;
                    case "System.String":
                        paramArray.Add(objValue.Data() as string);
                        break;
                    case "System.Boolean":
                        paramArray.Add(Boolean.Parse(objValue.Data() as string));
                        break;
                    default:	// pass Variable.Object
                        paramArray.Add(objValue);
                        break;
                }

                object res = type.InvokeMember(method.Data() as string, BindingFlags.Static | BindingFlags.Public | BindingFlags.SetProperty, null, obj, paramArray.ToArray());
                switch (res.GetType().ToString())
                {
                    case "System.Char":
                    case "System.String":
                    case "System.Int32":
                    case "System.Boolean":
                        objValue.Unify(new ConstantTerm(res.ToString()));
                        break;
                    default:
                        objValue.Unify(new ObjectTerm(res));
                        break;
                }
            }
            else
            {

                ArrayList paramArray = new ArrayList();
                Type t = obj.Data().GetType();
                PropertyInfo pInfo = t.GetProperty(method.Data() as string);

                if (pInfo == null)
                {
                    return false;
                }
                if (pInfo.CanWrite == false)
                {
                    return false;
                }

                switch (pInfo.PropertyType.ToString())
                {
                    case "System.Int32":
                        paramArray.Add(Int32.Parse(objValue.Data() as string));
                        break;
                    case "System.Char":
                        paramArray.Add(objValue.Data().ToString()[0]);
                        break;
                    case "System.String":
                        paramArray.Add(objValue.Data() as string);
                        break;
                    case "System.Boolean":
                        paramArray.Add(Boolean.Parse(objValue.Data() as string));
                        break;
                    default:	// pass Variable.Object
                        paramArray.Add(objValue);
                        break;
                }

                object res = obj.Data().GetType().InvokeMember(method.Data() as string, BindingFlags.Default | BindingFlags.ExactBinding | BindingFlags.SetProperty, null, obj.Data(), paramArray.ToArray());
                
            }
            return true;
        }

        public override int Arity()
        {
            return 3;
        }

        public override string Name()
        {
            return "set_property";
        }
    }
}
