using System;

namespace Hook.ConnectionsPredicates
{
    [Serializable]
    public class AlwaysTrue : IConnectionPredicate
    {
        public bool IsMet(Connection input)
        {
            return true;
        }
    }
}