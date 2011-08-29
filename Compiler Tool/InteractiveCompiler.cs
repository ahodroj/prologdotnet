using System;
using System.Collections;
using System.IO;
using Axiom.Compiler.Framework;
using Axiom.Runtime;

namespace Prolog.CT
{
	/// <summary>
	/// Summary description for PrologInteractiveCompiler.
	/// </summary>
	public class InteractiveCompiler : CompilerView
	{
		
		public InteractiveCompiler()
		{
            /* Compile */
            PrologCodeProvider provider = new PrologCodeProvider();
            IPrologCompiler compiler = provider.CreateCompiler();
            PrologCompilerParameters parameters = new PrologCompilerParameters();
            PrologCompilerResults results = compiler.CompileAbstractCodeFromFile(parameters, "boot.pro");
            
            /* Run */
            AbstractMachineState runtime = new AbstractMachineState(new AMFactory());
            
            //runtime.Init(results.AbstractInstructions, results.ForeignMethods, results.Namespaces, results.AssemblyFiles);
            runtime.Initialize(results.AbstractInstructions);
            runtime.Transition();
		}

		public InteractiveCompiler(string filename)
		{
            PrologCodeProvider provider = new PrologCodeProvider();
            IPrologCompiler compiler = provider.CreateCompiler();
            PrologCompilerParameters parameters = new PrologCompilerParameters();
            PrologCompilerResults results = compiler.CompileAbstractCodeFromFile(parameters, "boot.pro");

            /* Run */
            AbstractMachineState runtime = new AbstractMachineState(new AMFactory());
            //runtime.Init(results.AbstractInstructions, results.ForeignMethods, results.Namespaces, results.AssemblyFiles);
            runtime.Initialize(results.AbstractInstructions);
            
            runtime.Transition();
		}
	}
}
