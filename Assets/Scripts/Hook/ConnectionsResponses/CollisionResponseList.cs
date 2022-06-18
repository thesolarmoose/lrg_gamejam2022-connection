using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hook.ConnectionsResponses
{
    [Serializable]
    public class CollisionResponseList : ICollisionResponse
    {
        [SerializeReference, SubclassSelector] private List<ICollisionResponse> _responses;
        
        public void Execute(Collision collision)
        {
            foreach (var response in _responses)
            {
                response.Execute(collision);
            }
        }
    }
}