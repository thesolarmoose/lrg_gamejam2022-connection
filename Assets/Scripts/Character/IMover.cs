using UnityEngine;

namespace Character
{
    public interface IMover
    {
        Vector2 GetCurrentPosition();
        void Move(Vector2 dir);
    }
}