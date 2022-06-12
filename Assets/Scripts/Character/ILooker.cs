using UnityEngine;

namespace Character
{
    public interface ILooker
    {
        Vector2 CurrentLookDirection { get; }
        void Look(Vector2 dir);
    }
}