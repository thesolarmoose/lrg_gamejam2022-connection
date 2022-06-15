using DefaultNamespace;
using UnityEngine;

namespace Skills
{
    public class ShootBulletSkill : BaseDirectionalSkill
    {
        [SerializeField] public Bullet _bulletPrefab;
        [SerializeField] private float _cooldown;

        private float _lastTimeShot;

        private bool CanShoot => Time.time > _lastTimeShot + _cooldown;

        public override void Use(Vector2 dir)
        {
            if (CanShoot)
            {
                _lastTimeShot = Time.time;
                var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                bullet.Shoot(dir);
            }
        }
    }
}