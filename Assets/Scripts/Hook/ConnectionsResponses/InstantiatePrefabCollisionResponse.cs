using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hook.ConnectionsResponses
{
    [Serializable]
    public class InstantiatePrefabCollisionResponse : ICollisionResponse
    {
        [SerializeField] private GameObject _prefab;
        
        public void Execute(Collision collision)
        {
            Object.Instantiate(_prefab, collision.CollisionPoint, Quaternion.identity);
        }
    }
}