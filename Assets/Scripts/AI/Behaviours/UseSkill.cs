using System;
using System.Xml.Schema;
using Character;
using Skills;
using UnityEngine;

namespace AI.Behaviours
{
    public class UseSkill : AiBehaviour
    {
        [SerializeField] private BaseDirectionalSkill _skill;
        [SerializeField] private GunLooker _gun;

        private void Update()
        {
            var dir = _gun.CurrentLookDirection;
            _skill.Use(dir);
        }
    }
}