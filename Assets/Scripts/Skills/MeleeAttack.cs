using System;
using System.Collections.Generic;
using Character;
using UnityEngine;
using Utils.Extensions;

namespace Skills
{
    public class MeleeAttack : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _whoToDamage;

        private readonly List<Health> _alreadyDamaged = new List<Health>();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_whoToDamage.IsLayerInMask(other.gameObject.layer))
            {
                var health = other.gameObject.GetComponent<Health>();
                if (!_alreadyDamaged.Contains(health))
                {
                    health.Damage(_damage);       
                    _alreadyDamaged.Add(health);
                }
            }
        }

        private void DestroyAttack()
        {
            Destroy(gameObject);
        }
    }
}