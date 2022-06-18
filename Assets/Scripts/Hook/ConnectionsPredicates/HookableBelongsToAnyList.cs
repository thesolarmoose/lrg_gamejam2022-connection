using System;
using System.Collections.Generic;
using DummySOA;
using UnityEngine;

namespace Hook.ConnectionsPredicates
{
    [Serializable]
    public class HookableBelongsToAnyList : IConnectionPredicate
    {
        [SerializeField] private List<RuntimeGameObjectsList> _list;
        
        public bool IsMet(Connection connection)
        {
            foreach (var runtimeList in _list)
            {
                if (runtimeList.Contains(connection.Hooked.gameObject))
                    return true;
            }

            return false;
        }
    }
}