using System;


namespace Axiom.Runtime
{
    public class AbstractNonDeterministicPredicate
    {
        // predicate call ID
        protected int CALL_ID = -1;



        public void IncrementCallID()
        {
            CALL_ID++;
        }


    }
}
