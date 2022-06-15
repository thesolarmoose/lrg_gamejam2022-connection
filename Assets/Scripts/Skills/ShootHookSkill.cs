using Character;
using Hook;
using UnityEngine;

namespace Skills
{
    public class ShootHookSkill : BaseDirectionalSkill
    {
        [SerializeField] private GunLooker _gun;
        [SerializeField] private RetractableHook _hookPrefab;
        [SerializeField] private float _cooldown;

        private float _lastTimeShot;
        private bool _currentlyConnected;
        private RetractableHook _currentHook;

        private bool CanShoot => Time.time > _lastTimeShot + _cooldown;

        public override void Use(Vector2 dir)
        {
            if (_currentHook != null && _currentHook.IsStillConnected)
            {
                _currentHook.Shoot(dir, transform);
                _lastTimeShot = Time.time;
            }
            else if (CanShoot)
            {
                _currentHook = Instantiate(_hookPrefab);
                _currentHook.Initialize(transform.position);
                _currentHook.Shoot(dir, transform);
            }
        }
    }
}