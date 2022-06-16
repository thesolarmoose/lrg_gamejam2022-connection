using UnityEngine;

namespace Skills
{
    public class MeleeAttackSkill : BaseDirectionalSkill
    {
        [SerializeField] public MeleeAttack _attackPrefab;
        [SerializeField] private float _cooldown;

        private float _lastTimeShot;

        private bool CanShoot => Time.time > _lastTimeShot + _cooldown;

        public override void Use(Vector2 dir)
        {
            if (CanShoot)
            {
                _lastTimeShot = Time.time;
                var attack = Instantiate(_attackPrefab, transform.position, transform.rotation);
                attack.transform.localScale = transform.lossyScale;
            }
        }
    }
}