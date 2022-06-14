using UnityEngine;

namespace Hook
{
    public interface IHookable
    {
        bool IsMovable();
        Transform GetTransform();
        Collider2D GetCollider();
        
        void OnConnected();
        void OnStartRetracting();
        void OnCollided(IHookable other);
    }
}