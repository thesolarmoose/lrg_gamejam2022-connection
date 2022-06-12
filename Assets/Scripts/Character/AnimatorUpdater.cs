using System;
using UnityEngine;

namespace Character
{
    public class AnimatorUpdater : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("speed");
        
        [SerializeField] private CharacterMovement _character;
        [SerializeField] private Animator _animator;

        private void Update()
        {
            float speed = _character.CurrentSpeed;
            _animator.SetFloat(SpeedHash, speed);
        }
    }
}