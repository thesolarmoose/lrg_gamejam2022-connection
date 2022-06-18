using System;
using DummySOA;
using UnityEngine;

namespace Hook.ConnectionsPredicates
{
    [Serializable]
    public class HookableBelongsToList : IConnectionPredicate
    {
        [SerializeField] private RuntimeGameObjectsList _list;
        
        public bool IsMet(Connection connection)
        {
            bool contains = _list.Contains(connection.Hooked.gameObject);
            return contains;
        }
    }
}