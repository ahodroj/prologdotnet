using System;
using System.Collections;
using System.Text;

namespace Axiom.Runtime
{
    public class AMTrail : AbstractTrail
    {

        static AMTrail _instance = null;
        static readonly object padlock = new object();


        private ArrayList _trail;
        private int _tr;
        public int TR
        {
            get { return _tr; }
            set { _tr = value; }
        }

        AMTrail()
        {
            _trail = new ArrayList();
        }

        public override void Initialize()
        {
            _trail = new ArrayList();
        }

        public override bool Stop()
        {
            return true;
        }

        public override void Trail(AbstractTerm term)
        {
            _trail.Add(term);
            _tr++;
        }

        public void Unwind(int count)
        {
            if (count == 0)
            {
                return;
            }
            for (int i = count; i < _tr; i++)
            {
                AbstractTerm term = (AbstractTerm)_trail[i];
                term.Unbind();
            }

        }

        public static AMTrail Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AMTrail();
                    }
                    return _instance;
                }
            }
        }
    }
}
