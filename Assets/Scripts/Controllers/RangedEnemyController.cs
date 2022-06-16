using System;
using Character;
using Skills;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Object = System.Object;

namespace Controllers
{
    public class RangedEnemyController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement _character;
        [SerializeField] private BaseDirectionalSkill _shootSkill;
        [SerializeField] private string _playerTag;

        private Transform _player;

        private void Start()
        {
            _player = GameObject.FindWithTag(_playerTag).transform;
        }

        private void Update()
        {
            var charPos = _character.transform.position;
            var playerPos = _player.position;
            
            // move dir
            var moveDir = Vector2.zero;

            // look dir
            Vector2 lookDir = playerPos - charPos;
            lookDir.Normalize();
            
            _character.Move(moveDir);
            _character.Look(lookDir);
            
            _shootSkill.Use(lookDir);
        }
    }
}