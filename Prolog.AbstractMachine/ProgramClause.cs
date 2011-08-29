using Axiom.Runtime.Instructions;
using System;

namespace Axiom.Runtime
{
	/// <summary>
	/// Description of ProgramClause.
	/// </summary>
	public class ProgramClause : ProgramNode
	{
        private ProgramClause _nextPredicate;
        public ProgramClause NextPredicate
        {
            get { return _nextPredicate; }
            set { _nextPredicate = value; }
        }


		private string _name;
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		
		private int _arity;
		public int Arity {
			get { return _arity; }
			set { _arity = value; }
		}
		
		public ProgramClause()
		{
		}
		
		public ProgramClause(string name, int arity) {
			_name = name;
			_arity = arity;
            _instruction = new NopInstruction();
		}
		
		public ProgramClause(string name, int arity, AbstractInstruction instruction) {
			_instruction = instruction;
			_name = name;
			_arity = arity;
		}
	}
}
