using Character;
using UnityEngine;

namespace AI.Behaviours
{
    public class MoveFixed : AiBehaviour
    {
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private Vector2 _moveDir;

        private void Update()
        {
            _movement.Move(_moveDir);
        }
    }
}