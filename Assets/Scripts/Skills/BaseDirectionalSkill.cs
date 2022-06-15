using UnityEngine;

namespace Skills
{
    public abstract class BaseDirectionalSkill : MonoBehaviour, IDirectionalSkill
    {
        public abstract void Use(Vector2 dir);
    }
}