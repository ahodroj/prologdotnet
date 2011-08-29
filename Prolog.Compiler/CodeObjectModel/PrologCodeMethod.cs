//------------------------------------------------------------------------------
// <copyright file="PrologCodeMethod.cs" company="Axiom">
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
	/// Represents a foreign imported method.
	/// </summary>
	public class PrologCodeMethod
	:	PrologCodeTerm
	{
		private string _class;
		private string _assemblyName;
		private int _type;
		private ArrayList _arguments;
		private string _methodName;
        private string _predicateName;

        public PrologCodeMethod()
        {
            _class = "";
            _assemblyName = "";
            _arguments = new ArrayList();
            _methodName = "";
            _predicateName = "";
        }

		/// <summary>
		/// Initializes a new instance of the PrologCodeMethod class.
		/// </summary>
		/// <param name="_class">class type that contains the method.</param>
		/// <param name="_assemblyName">assembly name.</param>
		/// <param name="_type">return type of the method.</param>
		/// <param name="_methodName">method name.</param>
		/// <param name="_predicateName">predicate name.</param>
		public PrologCodeMethod (string _class, string _assemblyName, int _type, string _methodName, string _predicateName)
		{
		this._class = _class;
		this._assemblyName = _assemblyName;
		this._type = _type;
		this._methodName = _methodName;
		this._predicateName = _predicateName;
		this._arguments = new ArrayList();
		}
		
		/// <summary>
		/// Initializes a new instance of the PrologCodeMethod class.
		/// </summary>
		/// <param name="_class">class type that includes the method.</param>
		/// <param name="_assemblyName">assembly name.</param>
		/// <param name="_type">method return type.</param>
		/// <param name="_methodName">method name.</param>
		public PrologCodeMethod (string _class, string _assemblyName, int _type, string _methodName)
		{
			this._class = _class;
			this._assemblyName = _assemblyName;
			this._type = _type;
			this._methodName = _methodName;
			this._predicateName = _methodName;
			this._arguments = new ArrayList();
		}
		
		/// <summary>
		/// Gets or sets the class type.
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
		/// Gets or sets the method reutrn type.
		/// </summary>
		public int Type
		{
			get
			{
				return this._type;
			}
			
			set
			{
				this._type = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the collection of method arguments.
		/// </summary>
		public ArrayList Arguments
		{
			get
			{
				return this._arguments;
			}
			
			set
			{
				this._arguments = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the method name.
		/// </summary>
		public string MethodName
		{
			get
			{
				return this._methodName;
			}
			
			set
			{
				this._methodName = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the predicate name.
		/// </summary>
		public string PredicateName
		{
			get
			{
				return this._predicateName;
			}
			
			set
			{
				this._predicateName = value;
			}
		}
	}
}