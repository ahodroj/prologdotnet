//------------------------------------------------------------------------------
// <copyright file="PrologVariableDictionaryEntry.cs" company="Axiom">
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


	public class PrologVariableDictionaryEntry
	{

		public PrologVariableDictionaryEntry(string name, int temporaryIndex) 
		{
			_name = name;
			_temporaryIndex = temporaryIndex;
		}

		private string _name;
		public string Name 
		{
			get { return _name; }
			set { _name = value; }
		}

		private int _occurrences = 0;
		public int Occurrences 
		{
			get { return _occurrences; }
			set { _occurrences = value; }
		}

		private bool _isTemporary = false;
		public bool IsTemporary 
		{
			get { return _isTemporary; }
			set { _isTemporary = value; }
		}

		private bool _isPermanent = false;
		public bool IsPermanent 
		{
			get { return _isPermanent; }
			set { _isPermanent = value; }
		}

		private bool _isUnsafe = false;
		public bool IsUnsafe 
		{
			get { return _isUnsafe; }
			set { _isUnsafe = value; }
		}

		private bool _isReferenced = false;
		public bool IsReferenced 
		{
			get 
			{
				if(_isReferenced) 
				{
					return true;
				}
				_isReferenced = true;
				if(_isTemporary && _temporaryIndex == -1) 
				{
					PrologRegisterTable regs = PrologRegisterTable.Instance;
					_temporaryIndex = regs.FindRegister();
				}
				return false;
			}
			set 
			{
				_isReferenced = value;
			}
		}
	
	
		private bool _isGlobal = false;
		public bool IsGlobal 
		{
			get { return _isGlobal; }
			set { _isGlobal = value; }
		}

		private int _firstGoal = -1;
		public int FirstGoal 
		{
			get { return _firstGoal; }
			set { _firstGoal = value; }
		}

		private int _lastGoal = -1;
		public int LastGoal 
		{
			get { return _lastGoal; }
			set { _lastGoal = value; }
		}

		private int _lastGoalArgument = -1;
		public int LastGoalArgument 
		{
			get { return _lastGoalArgument; }
			set { _lastGoalArgument = value; }
		}

		private int _temporaryIndex = -1;
		public int TemporaryIndex 
		{
			get { return _temporaryIndex; }
			set { _temporaryIndex = value; }
		}

		private int _permanentIndex = -1;
		public int PermanentIndex 
		{
			get { return _permanentIndex; }
			set { _permanentIndex = value; }
		}
	}
}