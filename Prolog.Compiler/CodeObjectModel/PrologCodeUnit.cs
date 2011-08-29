//------------------------------------------------------------------------------
// <copyright file="PrologCodeUnit.cs" company="Axiom">
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

namespace Axiom.Compiler.CodeObjectModel
{	
	/// <summary>
	/// Provides a container for a code object model of a Prolog program.
	/// </summary>
	public class PrologCodeUnit
	{
		private ArrayList _methods;
		private ArrayList _operators;
        private ArrayList _namespaces;
        private ArrayList _assemblyFiles;
		private string _assemblyName;
		private string _class;
        private ArrayList _terms;
		
		/// <summary>
		/// Initializes a new instance of PrologCodeUnit class.
		/// </summary>
		public PrologCodeUnit ()
		{
			_methods = new ArrayList();
			_operators = new ArrayList();
            _terms = new ArrayList();
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
		
		/// <summary>
		/// Gets a collection of imported methods.
		/// </summary>
		public ArrayList Methods
		{
			get
			{
				return this._methods;
			}
		}
		
		/// <summary>
		/// Gets a collection of new or modified operators.
		/// </summary>
		public ArrayList Operators
		{
			get
			{
				return this._operators;
			}
		}
		
		/// <summary>
		/// Gets or sets the assembly name.
		/// </summary>
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			
			set
			{
				this._assemblyName = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the class name to compile to.
		/// </summary>
		public string Class
		{
			get
			{
				return this._class;
			}
			
			set
			{
				this._class = value;
			}
		}

        public ArrayList Terms
        {
            get { return _terms; }
        }
	}
}