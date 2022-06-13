using Hook;
using UnityEngine;

namespace Character
{
    public class ShootSkill : MonoBehaviour
    {
        [SerializeField] private GunLooker _gun;
        [SerializeField] private RetractableHook _hookPrefab;
        [SerializeField] private float _cooldown;

        private float _lastTimeShot;
        private bool _currentlyConnected;
        private RetractableHook _currentHook;

        private bool CanShoot => Time.time > _lastTimeShot + _cooldown;

        public void Shoot()
        {
            if (_currentHook != null && _currentHook.IsStillConnected)
            {
                _currentHook.Shoot(_gun.CurrentLookDirection, transform);
                _lastTimeShot = Time.time;
            }
            else if (CanShoot)
            {
                _currentHook = Instantiate(_hookPrefab);
                _currentHook.Initialize(transform.position);
                _currentHook.Shoot(_gun.CurrentLookDirection, transform);
            }
        }
    }
}