using System;
using System.Collections;
using System.Text;
using System.Reflection;

using Axiom.Runtime.Instructions;
using Axiom.Runtime.Builtins.IO;
using Axiom.Runtime.Builtins.Comparison.Numeric;
using Axiom.Runtime.Builtins.Control;
using Axiom.Runtime.Builtins.Equality;
using Axiom.Runtime.Builtins.Meta;
using Axiom.Runtime.Builtins.OOP;
using Axiom.Runtime.PluginHost;


namespace Axiom.Runtime
{

    public sealed class AMPredicateSet
    {
        private Hashtable _predicates = new Hashtable();
        static AMPredicateSet _instance = null;
        static readonly object padlock = new object();

        AMPredicateSet()
        {
            // Initialize local predicates
            InstallLocalPredicates();

            // Initialize remote predicates/plugins
            /* Deprecated for demo release 
             
            InstallExternalPredicates();
             */
        }

        private void InstallExternalPredicates()
        {
            AMPluginManager p = new AMPluginManager();

            foreach (ExternalPredicate e in p.Plugins)
            {
                IAbstractMachinePredicate predicate = LoadExternalPredicate(e.Path, e.Name);
                if (predicate == null)
                {
                    throw new Exception("Remote predicate " + e.Name + " does not implement correct interface.");
                }

                if (!e.Name.Equals(predicate.Name() + "/" + predicate.Arity().ToString()))
                {
                    throw new Exception("Cannot install corrupted remote predicate: " + e.Name);
                }

                // Add new external predicate
                _predicates[e.Name] = predicate;
            }
            
        }

        private IAbstractMachinePredicate LoadExternalPredicate(string path, string pname)
        {
            // Load assembly 
            Assembly assembly = Assembly.LoadFrom(path);

            if (assembly == null)
            {
                throw new Exception("Error loading external predicate from " + path);
            }

            // Verify that the class implements IAbstractMachinePredicate
            foreach (Type classType in assembly.GetTypes())
            {
                foreach (Type implementedInterface in classType.GetInterfaces())
                {
                    if (implementedInterface.ToString() == "Axiom.Runtime.IAbstractMachinePredicate")
                    {
                        object [] attributes = classType.GetCustomAttributes(typeof(PredicateNameAttribute), false);
                        
                        if (attributes != null)
                        {
                            if (((PredicateNameAttribute)attributes[0]).Name == pname)
                            {
                                // create and return instance of this class type
                                return (IAbstractMachinePredicate)Activator.CreateInstance(classType.UnderlyingSystemType);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void InstallLocalPredicates()
        {
            // Input/Output
            _predicates["write/1"] = new WritePredicate();
            _predicates["writeln/1"] = new WriteLnPredicate();
            _predicates["nl/0"] = new NlPredicate();
            _predicates["get0/1"] = new Get0Predicate();
            _predicates["skip/1"] = new SkipPredicate();
            _predicates["put/1"] = new PutPredicate();

            // Comparison, numeric
            _predicates["=\\=/2"] = new NotEqualsPredicate();
            _predicates["=:=/2"] = new EqualsPredicate();
            _predicates[">=/2"] = new GreaterThanEqualPredicate();
            _predicates[">/2"] = new GreaterThanPredicate();
            _predicates["=</2"] = new LessThanEqualPredicate();
            _predicates["</2"] = new LessThanPredicate();

            // Control
            _predicates["call/1"] = new CallPredicate();

            // Equality
            _predicates["=/2"] = new UnifyPredicate();
            _predicates["\\=/2"] = new NotUnifiablePredicate();

            // Meta
            _predicates["is/2"] = new IsPredicate();
            _predicates["atom/1"] = new AtomPredicate();
            _predicates["bound/1"] = new BoundPredicate();
            _predicates["char/1"] = new CharPredicate();
            _predicates["free/1"] = new FreePredicate();
            _predicates["integer/1"] = new IntegerPredicate();
            _predicates["nonvar/1"] = new NonVarPredicate();
            _predicates["var/1"] = new VarPredicate();

            // Object-Oriented Programming
            _predicates["object/2"] = new object_2();
            _predicates["invoke/3"] = new invoke_2();
            _predicates["get_property/3"] = new get_property_3();

            // .NET Reflection
            
            
        }

        public bool IsValidPredicate(string predicateName)
        {
            return _predicates.ContainsKey(predicateName);
        }


        public IAbstractMachinePredicate CreatePredicate(string predicateName)
        {
            IAbstractMachinePredicate p = (IAbstractMachinePredicate)_predicates[predicateName];
            return (IAbstractMachinePredicate)Activator.CreateInstance(p.GetType());
        }

        public static AMPredicateSet Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AMPredicateSet();
                    }
                    return _instance;
                }
            }
        }

        public bool IsBuiltin(string predicateName, int arity)
        {
            return IsValidPredicate(predicateName + "/" + arity);
        }

        public object CreatePredicate(string predicateName, int arity)
        {
            return CreatePredicate(predicateName + "/" + arity);
        }
    }


   
}
