
using System;
using System.Collections;


namespace Axiom.Runtime
{
	/// <summary>
	/// Description of Choicepoint.
	/// </summary>
	public class Choicepoint : HeapNode
	{
		private int _arity;
		public int Arity {
			get { return _arity; }
			set { _arity = value; }
		}


        private ArrayList _savedVariables = new ArrayList();
        public ArrayList SaveVariables
        {
            get { return _savedVariables; }
        }
		
		
		
		private EnvironmentFrame _ce;
		public EnvironmentFrame CE {
			get { return _ce; }
			set { _ce = value; }
		}
		
		private ProgramNode _cp;
		public ProgramNode CP {
			get { return _cp; }
			set { _cp = value; }
		}
		
		private Choicepoint _b;
		public Choicepoint B {
			get { return _b; }
			set { _b = value; }
		}
		
		private ProgramClause _nextClause;
		public ProgramClause NextClause {
			get { return _nextClause; }
			set { _nextClause = value; }
		}
		
		private int _tr;
		public int TR {
			get { return _tr; }
			set { _tr = value; }
		}
		
		private HeapNode _h;
		public HeapNode H {
			get { return _h; }
			set { _h = value; }
		}
		
		// TODO: B0 cut pointer goes here
		
		
		public Choicepoint()
		{
			
		}
		
		public Choicepoint(int arity, 
		                   EnvironmentFrame ce, 
		                   ProgramNode cp, 
		                   Choicepoint b,
		                   ProgramClause nextClause,
		                   int tr,
		                   HeapNode h) {
                               _arity = arity;
                               _ce = ce;
                               _cp = cp;
                               _b = b;
                               _nextClause = nextClause;
                               _tr = tr;
                               _h = h;
		}

        public AbstractTerm this[string item]
        {
            get { return GetChoicepointItem(item); }

        }

        private AbstractTerm GetChoicepointItem(string item)
        {
            if (item[0] == 'X')
            {
                int index = Int32.Parse(item.Remove(0, 1));
                return (AbstractTerm)_savedVariables[index];
            }
            else
            {
                return null;
            }
           
        }
        public void SaveVariable(AbstractTerm register)
        {
        }

        public void SaveRegisters(AbstractMachineState state, int count)
        {
            for (int i = 0; i < count; i++)
            {
                AbstractTerm toSave = new AbstractTerm();
                toSave.Copy((AbstractTerm)state["X" + i.ToString()]);
                _savedVariables.Add(toSave);
            }
        }

        public void UnsaveRegisters(AbstractMachineState state, int count)
        {
            for (int i = 0; i < count; i++)
            {
                AbstractTerm Xi = (AbstractTerm)state["X" + i.ToString()];
                Xi.Assign((AbstractTerm)_savedVariables[i]);
            }
        }
    }

}
