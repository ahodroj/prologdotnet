using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
    public class EnvironmentFrame : HeapNode
    {
        private int _variableCount = 0;

        private Choicepoint _b0;
        public Choicepoint B0
        {
            get { return _b0; }
            set { _b0 = value; }
        }

        private EnvironmentFrame _ce;
        public EnvironmentFrame CE
        {
            get { return _ce; }
            set { _ce = value; }
        }

        private ProgramNode _cp;
        public ProgramNode CP
        {
            get { return _cp; }
            set { _cp = value; }
        }

        private AbstractTerm _permanentVariables;
        public AbstractTerm PermanentVariables {
        	get { return _permanentVariables; }
        }
        
        private AbstractTerm _permanentVariablesTop;

        public EnvironmentFrame()
        {
        }
        
        public EnvironmentFrame(EnvironmentFrame ce, ProgramNode cp, int size) {
        	_ce = ce;
        	_cp = cp;
        	for(int i = 0; i < size; i++) {
        		AddVariable();
        	}
        }

        public EnvironmentFrame(EnvironmentFrame ce, ProgramNode cp, Choicepoint b0, int size)
        {
            _ce = ce;
            _cp = cp;
            _b0 = b0;
            for (int i = 0; i < size; i++)
            {
                AddVariable();
            }
        }
        
        public void AddVariable() {
        	if(_permanentVariablesTop == null) {
        		_permanentVariablesTop = new AbstractTerm();
        		_permanentVariables = _permanentVariablesTop;
        	} else {
        		_permanentVariablesTop.Next = new AbstractTerm();
        		_permanentVariablesTop = (AbstractTerm)_permanentVariablesTop.Next;
        	}
            _variableCount++;
        }

        public AbstractTerm this[string yName]
        {
        	get 
        	{
        		int i = Int32.Parse(yName.Remove(0,1));
                return GetPermanentRegister(i);
        	}
        }

        private AbstractTerm GetPermanentRegister(int i)
        {
            if (i == 0)
            {
                return _permanentVariables;
            }
            else
            {
                if (_variableCount < i + 1)
                {
                    
                    for (int n = 0; n < (i - _variableCount + 10); n++)
                    {
                        AddVariable();
                    }
                }
                AbstractTerm vPtr = _permanentVariables;
                for (int j = 0; j < i; j++)
                {
                    vPtr = (AbstractTerm)vPtr.Next;
                }
                return vPtr;
            }
        }

        public override string ToString()
        {
            return "<env>";
        }
    }
}
