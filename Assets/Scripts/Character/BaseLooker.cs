using UnityEngine;

namespace Character
{
    public abstract class BaseLooker : MonoBehaviour, ILooker
    {
        public Vector2 CurrentLookDirection { get; private set; }
        
        public void Look(Vector2 dir)
        {
            CurrentLookDirection = dir;
            _Look(dir);
        }

        protected abstract void _Look(Vector2 dir);
    }
}