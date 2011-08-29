//------------------------------------------------------------------------------
// <copyright file="PrologRegisterTable.cs" company="Axiom">
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


namespace Axiom.Compiler.Framework
{
	using System;
	using System.Collections;

	public class PrologRegisterTable
	{
		private ArrayList _registers = new ArrayList();


		public PrologRegisterTable() 
		{
			for(int i = 0; i < 25; i++) 
			{
				_registers.Add(false);
			}
		}

		public void AllocateRegister(int reg) 
		{
			_registers[reg] = true;
		}

		public void FreeRegister(int reg) 
		{
			_registers[reg] = false;
		}

		public void FreeAllRegisters() 
		{
			for(int i = 0; i < _registers.Count; i++) 
			{
				_registers[i] = false;
			}
		}

		public int FindRegister() 
		{
			for(int i = 0; i < _registers.Count; i++) 
			{
				if(!InUse(i)) 
				{
					AllocateRegister(i);
					return i;
				}
			}
			return 0;
		}

		public bool InUse(int reg) 
		{
			return (bool)_registers[reg];
		}

		static PrologRegisterTable _instance = null;
		public static PrologRegisterTable Instance 
		{
			get 
			{
				lock(padlock) 
				{
					if(_instance == null) 
					{
						_instance = new PrologRegisterTable();
					}
				}
				return _instance;
			}
            set
            {
                _instance = value;
            }
		}

		static readonly object padlock = new object();

		
	}

}