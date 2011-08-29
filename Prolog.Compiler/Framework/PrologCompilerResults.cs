//------------------------------------------------------------------------------
// <copyright file="PrologCompilerResults.cs" company="Axiom">
//     
//      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
//     
//      The use and distribution terms for this source code are contained in the file
//      named license.txt, which can be found in the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by the
//      terms of this license.
//     
//      You must not remove this notice, or any other, from this software.
//     
// </copyright>                                                                
//------------------------------------------------------------------------------



using System;
using System.Collections;
using System.Reflection;

namespace Axiom.Compiler.Framework
{	
	public class PrologCompilerResults
	{
		private Assembly _compiledAssembly;
		private ArrayList _errors;
        private ArrayList _abstractInstructions;
        private ArrayList _foreignMethods;
        private ArrayList _namespaces;
        private ArrayList _assemblyFiles;
		
		public PrologCompilerResults ()
		{
			_errors = new ArrayList();
            _abstractInstructions = new ArrayList();
            _foreignMethods = new ArrayList();
            _namespaces = new ArrayList();
            _assemblyFiles = new ArrayList();
		}

        public ArrayList Namespaces
        {
            get { return _namespaces; }
            set { _namespaces = value; }
        }

        public ArrayList AssemblyFiles
        {
            get { return _assemblyFiles; }
            set { _assemblyFiles = value; }
        }

        public ArrayList ForeignMethods
        {
            get { return _foreignMethods; }
            set { _foreignMethods = value; }
        }

		public Assembly CompiledAssembly
		{
			get
			{
				return this._compiledAssembly;
			}
			
			set
			{
				this._compiledAssembly = value;
			}
		}

        public ArrayList AbstractInstructions
        {
            get { return _abstractInstructions; }
            set { _abstractInstructions = value; }
        }

		public ArrayList Errors
		{
			get
			{
				return this._errors;
			}
            set
            {
                _errors = value;
            }
		}
	}
}