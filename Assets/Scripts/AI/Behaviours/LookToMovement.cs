using System;
using Character;
using UnityEngine;

namespace AI.Behaviours
{
    public class LookToMovement : AiBehaviour
    {
        [SerializeField] private CharacterMovement _movement;

        private void Update()
        {
            var lookDir = _movement.CurrentVelocity.normalized;
            _movement.Look(lookDir);
        }
    }
}