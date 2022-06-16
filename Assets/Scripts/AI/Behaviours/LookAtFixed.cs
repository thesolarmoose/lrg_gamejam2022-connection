using Character;
using UnityEngine;

namespace AI.Behaviours
{
    public class LookAtFixed : AiBehaviour
    {
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private Vector2 _lookAt;

        private void Update()
        {
            var xScale = _movement.transform.lossyScale.x;
            var dir = _lookAt.normalized;
            dir.x *= xScale;
            _movement.Look(dir);
        }
    }
}