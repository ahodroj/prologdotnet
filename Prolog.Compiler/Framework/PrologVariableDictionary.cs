//------------------------------------------------------------------------------
// <copyright file="PrologVariableDictionary.cs" company="Axiom">
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
	using Axiom.Compiler.CodeObjectModel;


	// This class implements a simple dictionary using an array of DictionaryEntry objects (key/value pairs).
	public class PrologVariableDictionary : IDictionary
	{
		
		private ArrayList _items;
		
		private int _temporaryVariableCount = 0;
		public int TemporaryVariableCount 
		{
			get { return _temporaryVariableCount; }
		}

		private PrologRegisterTable _registers = PrologRegisterTable.Instance;

		private int _goalCount = 0;
		public int GoalCount 
		{
			get { return _goalCount; }
		}

		private int _currentArgumentIndex = 0;
		public int CurrentArgumentIndex
		{
			get { return _currentArgumentIndex; }
			set { _currentArgumentIndex = value; }
		}

		private int _currentGoalIndex = 0;
		public int CurrentGoalIndex 
		{
			get { return _currentGoalIndex; }
			set { _currentGoalIndex = value; }
		}

		public PrologVariableDictionary() 
		{
			_items = new ArrayList();
		}
		// Construct the SimpleDictionary with the desired number of items.
		// The number of items cannot change for the life time of this SimpleDictionary.
		public PrologVariableDictionary(int numItems)
		{
			_items = new ArrayList(numItems);
		}


		#region IDictionary Members

		public bool IsReadOnly
		{
			get { return false; } 
		}

		public bool Contains(object key)
		{
			foreach(PrologVariableDictionaryEntry e in _items) 
			{
				if(e.Name == (string)key) 
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFixedSize
		{
			get { return false; } 
		}

		public void Remove(object key)
		{
			if (key == null)  
			{
				throw new ArgumentNullException("key");
			}

			string name = (string)key;
			for(int i = 0; i < _items.Count; i++) 
			{
				PrologVariableDictionaryEntry entry = (PrologVariableDictionaryEntry)_items[i];
				if(entry.Name == name) 
				{
					_items.RemoveAt(i);
					return;
				}
			}
		}

		public void Clear() 
		{
			_items.Clear();
		}

		public void Add(object key, object val) 
		{
			string name = (string)key;
           
			
			if(this.Contains(key)) 
			{
				PrologVariableDictionaryEntry entry = GetEntry((string)key);
				entry.Occurrences += 1;
				entry.LastGoal = _currentGoalIndex;
				entry.LastGoalArgument = _currentArgumentIndex;
			} 
			else 
			{
				PrologVariableDictionaryEntry variable = new PrologVariableDictionaryEntry(name, -1);
				variable.Occurrences = 0;
				variable.FirstGoal = _currentGoalIndex;
				variable.IsReferenced = false;
				variable.IsGlobal = false;
				variable.Occurrences += 1;
				variable.LastGoal = _currentGoalIndex;
				variable.LastGoalArgument = _currentArgumentIndex;
				variable.IsReferenced = false;
				_items.Add(variable);
			}
		}

		public ICollection Keys
		{
			get
			{
				// Return an array where each item is a key.
				Object[] keys = new Object[_items.Count];
				for (int n = 0; n < _items.Count; n++)
					keys[n] = ((PrologVariableDictionaryEntry)_items[n]).Name;
				return keys;
			}
		}

		public ICollection Values
		{
			get
			{
				return _items.ToArray();
			}
		}

		public object this[object key]
		{
			get
			{   
				foreach(PrologVariableDictionaryEntry entry in _items) 
				{
					if(entry.Name == (string)key) 
					{
						return entry;
					}
				}
				return null;
			}

			set
			{
				// This method is not implemented
			}
		}
		
		private PrologVariableDictionaryEntry GetEntry(string name) 
		{
			foreach(PrologVariableDictionaryEntry e in _items) 
			{
				if(e.Name == name) 
				{
					return e;
				}
			}
			return null;
		}
		
		private class PrologVariableDictionaryEnumerator : IDictionaryEnumerator
		{
			// A copy of the SimpleDictionary object's key/value pairs.
			ArrayList items = new ArrayList();
			int index = -1;

			public PrologVariableDictionaryEnumerator(PrologVariableDictionary dict)
			{
                items.AddRange(dict._items);

                /************** BUG: This causes a StackOverflow ********************
				foreach(PrologVariableDictionaryEntry e in dict) 
				{
					items.Add(e);
				}
                 *******************************************************************/
			}

			// Return the current item.
			public Object Current 
			{
				get { ValidateIndex(); return items[index]; } 
			}

			// Return the current dictionary entry.
			public DictionaryEntry Entry
			{
				get 
				{ 
					DictionaryEntry e = new DictionaryEntry(null,null);
					return e; 
				}
			}

			// Return the key of the current item.
			public Object Key 
			{ 
				get 
				{ 
					ValidateIndex();  
					return ((PrologVariableDictionaryEntry)items[index]).Name; 
				}
			}

			// Return the value of the current item.
			public Object Value 
			{
				get  
				{
					ValidateIndex();  
					return items[index]; 
				}
			}

			// Advance to the next item.
			public bool MoveNext()
			{
				if (index < items.Count - 1) 
				{
					index++;
					return true;
				}
				return false;
			}

			// Validate the enumeration index and throw an exception if the index is out of range.
			private void ValidateIndex()
			{
				if (index < 0 || index >= items.Count)
					throw new InvalidOperationException("Enumerator is before or after the collection.");
			}

			// Reset the index to restart the enumeration.
			public void Reset()
			{
				index = -1;
			}
		}
		public IDictionaryEnumerator GetEnumerator()
		{
			// Construct and return an enumerator.
			return new PrologVariableDictionaryEnumerator(this);
		}
		#endregion

		#region ICollection Members

		public bool IsSynchronized 
		{
			get { return false; }
		}

		public object SyncRoot
		{
			get { throw new NotImplementedException(); } 
		}

		public int Count 
		{ 
			get { return _items.Count; } 
		}

		public void CopyTo(Array array, int index) 
		{ 
			throw new NotImplementedException(); 
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator() 
		{
			// Construct and return an enumerator.
			return ((IDictionary)this).GetEnumerator();
		}
		#endregion

		#region Prolog Dictionary Operations

		public void Build(PrologCodeClause clause) 
		{
			/* reset goal count */
			_goalCount = 0;

			// reset everything.
			this.Reset();

			// set current goal index
			_currentGoalIndex = 0;

			// store head variables
			AddGoalVariables(clause.Head);
			_currentGoalIndex++;

			// build clause body goals
			foreach(PrologCodeTerm goal in clause.Goals) 
			{
				AddGoalVariables(goal);
				_currentGoalIndex++;
			}

			// set goal count to where we reached so far
			_goalCount = _currentGoalIndex;

			// Mark temporary variables
			MarkTemporaryVariables();

			// Mark permanent variables
			MarkPermanentVariables();

		}

		private void MarkTemporaryVariables() 
		{
			foreach(PrologVariableDictionaryEntry entry in _items) 
			{
				if(entry.LastGoal <= 1 || entry.FirstGoal == entry.LastGoal) 
				{
					entry.IsTemporary = true;
				}

				if(!entry.IsTemporary && entry.FirstGoal != 0) 
				{
					entry.IsUnsafe = true;
				}

				if(entry.IsTemporary) 
				{
					_temporaryVariableCount++;
				}
			}
		}

		private void MarkPermanentVariables() 
		{
			int nPermVars = 0;
			int goalN = 0;
			int index = 0;

			for(nPermVars = (_items.Count - _temporaryVariableCount), goalN = _goalCount - 1, index = 0;
				nPermVars > 0;
				goalN--) 
			{
				foreach(PrologVariableDictionaryEntry entry in _items) 
				{
					if(!entry.IsTemporary && entry.LastGoal == goalN) 
					{
						entry.PermanentIndex = index++;
						nPermVars--;
					}
				}
			}
		}

		public void ClearTempIndexOfPermanentVariables() 
		{
			foreach(PrologVariableDictionaryEntry entry in _items) 
			{
				if(!entry.IsTemporary) 
				{
					entry.TemporaryIndex = -1;
				}
			}
		}

		private void AddGoalVariables(PrologCodeTerm term) 
		{
			// no variables to add: predicate/0
			if(PrologCodeTerm.IsAtom(term) || PrologCodeTerm.IsAtomicPredicate(term)) 
			{
				return;
			}
			// goal is a variable X
			else if(PrologCodeTerm.IsVariable(term)) 
			{
				_currentArgumentIndex = 0;
				this.Add(((PrologCodeVariable)term).Name,null);
				return;
			}
			// goal is a list, [Term|Term]
			else if(PrologCodeTerm.IsList(term)) 
			{
				_currentArgumentIndex = 0;
				if(term is PrologCodeNonEmptyList) 
				{
					PrologCodeNonEmptyList list = (PrologCodeNonEmptyList)term;
					AddGoalArgumentVariables(list.Head);
					_currentArgumentIndex = 1;
					if(list.Tail != null) 
					{
						if(list.Tail is PrologCodeNonEmptyList) 
						{
							AddGoalArgumentVariables(list.Tail);
						} 
						else 
						{
							AddGoalArgumentVariables(list.Tail);
						}
					}
				}
			} 
			// Goal is a predicate, term(term,...)
			else if(PrologCodeTerm.IsStruct(term)) 
			{
				_currentArgumentIndex = 0;
				PrologCodePredicate goal = (PrologCodePredicate)term;
				foreach(PrologCodeTerm argument in goal.Arguments) 
				{
					AddGoalArgumentVariables(argument);
					_currentArgumentIndex++;
				}
			}
		}

		private void AddGoalArgumentVariables(PrologCodeTerm term) 
		{
			// goal(atom).
			if(PrologCodeTerm.IsAtom(term)) 
			{
				return;
			}
			// goal(X).
			else if(PrologCodeTerm.IsVariable(term)) 
			{
				this.Add(((PrologCodeVariable)term).Name, null);
				return;
			}
			// goal([A|B]).
			else if(PrologCodeTerm.IsList(term)) 
			{
				if(term is PrologCodeNonEmptyList) 
				{
					PrologCodeNonEmptyList list = (PrologCodeNonEmptyList)term;
					AddGoalStructArgumentVariables(list.Head);
                    if (list.Tail is PrologCodeNonEmptyList)
                    {
                        AddGoalStructArgumentVariables(list.Tail);
                    }
                    else
                    {
                        AddGoalStructArgumentVariables(list.Tail);
                    }
				}
			} 
			else if(PrologCodeTerm.IsStruct(term)) 
			{
                PrologCodePredicate goal = (PrologCodePredicate)term;
                foreach (PrologCodeTerm argument in goal.Arguments)
                {
                    AddGoalStructArgumentVariables(argument);
                }
			}
		}

        private void AddGoalStructArgumentVariables(PrologCodeTerm term)
        {
            if (PrologCodeTerm.IsAtom(term))
            {
                return;
            }
            else if (PrologCodeTerm.IsVariable(term))
            {
                this.Add(((PrologCodeVariable)term).Name, null);
            }
            else if (PrologCodeTerm.IsList(term))
            {
                if (term is PrologCodeNonEmptyList)
                {
                    PrologCodeNonEmptyList list = (PrologCodeNonEmptyList)term;
                    AddGoalStructArgumentVariables(list.Head);
                    if (list.Tail is PrologCodeNonEmptyList)
                    {
                        AddGoalStructArgumentVariables(list.Tail);
                    }
                    else
                    {
                        AddGoalStructArgumentVariables(list.Tail);
                    }
                }
            }
            else if (PrologCodeTerm.IsStruct(term))
            {
                PrologCodePredicate structure = (PrologCodePredicate)term;
                foreach (PrologCodeTerm argument in structure.Arguments)
                {
                    AddGoalStructArgumentVariables(argument);
                }
            }
        }

        public void Reset()
        {
            _items.Clear();
            ClearTempIndexOfPermanentVariables();
            PrologRegisterTable.Instance = null;
            _registers = PrologRegisterTable.Instance;
        }

        public void AllocateTemporaryVariable(PrologVariableDictionaryEntry entry, int reg)
        {
            _registers.AllocateRegister(reg);
            entry.TemporaryIndex = reg;
        }

        public void AllocatePermanentVariable(PrologVariableDictionaryEntry entry, int reg)
        {
            entry.PermanentIndex = reg;
        }

        public PrologVariableDictionaryEntry GetVariable(string name)
        {
            if (Contains(name))
            {
                return GetEntry(name);
            }
            return null;
        }

        public PrologVariableDictionaryEntry GetVariable(int register)
        {
            foreach (PrologVariableDictionaryEntry entry in _items)
            {
                if (entry.TemporaryIndex == register)
                {
                    return entry;
                }
            }
            return null;
        }
        
		#endregion

        public bool InLastGoal
        {
            get { return _currentGoalIndex == _goalCount - 1; }
        }
	}


}