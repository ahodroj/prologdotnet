using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Axiom.Runtime;


namespace Axiom.Runtime.Builtins.OOP
{
    // object(X0,X1).
    public class object_2 : AbstractPredicate
    {
        private string fullTypeName;

        public override void Execute(AbstractMachineState state)
        {
            AMProgram prog = (AMProgram)state.Program;

            AbstractTerm X0 = ((AbstractTerm)state["X0"]).Dereference();
            AbstractTerm X1 = ((AbstractTerm)state["X1"]).Dereference();

            if (!X0.IsConstant)
            {
                Console.WriteLine("Invalid class type of object/2");
                state.Backtrack();
            }

            if (X1.IsConstant)
            {
                Console.WriteLine("object/2: object instantiation error.");
                state.Backtrack();
            }

            if (CreateObject(X0.Data() as string, X1, state))
            {
                prog.Next();
            }
            else
            {
                state.Backtrack();
            }
        }

        private bool CreateObject(string classType, AbstractTerm netObject, AbstractMachineState state)
        {
            Assembly asm = GetRequiredAssembly(classType, state);
            Type[] types = asm.GetTypes();
            Type type = null;
            
            type = asm.GetType(fullTypeName);
            if (type == null)
            {
                return false;
            }
            netObject.Assign(new ObjectTerm(Activator.CreateInstance(type)));
            return true;
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
                    fullTypeName = fullName;
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

        public override int Arity()
        {
            return 2;
        }

        public override string Name()
        {
            return "object";
        }
    }
}
