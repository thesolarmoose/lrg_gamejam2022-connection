using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;

        public UnityEvent eventLifeSet;
        public UnityEvent<int> eventDamaged;
        public UnityEvent<int> eventHealed;
        public UnityEvent eventDied;
        
        private int _currentHealth;

        [ShowNativeProperty]
        public bool Invulnerable { get; set; }
        
        [ShowNativeProperty]
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                eventLifeSet?.Invoke();
            }
        }

        public int MaxHealth => _maxHealth;

        [ShowNativeProperty]
        public bool IsDead => CurrentHealth <= 0;
        
        private void Awake()
        {
            CurrentHealth = _maxHealth;
        }

        public void DamageNoReturn(int damage)
        {
            Damage(damage);
        }
        
        public int Damage(int damage)
        {
            if (Invulnerable || _currentHealth <= 0)
                return 0;
        
            int damageDone = Math.Min(damage, CurrentHealth);
            CurrentHealth -= damageDone;

            if (damageDone > 0)
            {
                eventDamaged?.Invoke(damageDone);
            }

            if (CurrentHealth <= 0)
            {
                eventDied?.Invoke();
            }

            return damageDone;
        }

        public void Heal(int heal)
        {
            int toMax = _maxHealth - CurrentHealth;
            int healDone = Math.Min(heal, toMax);
            CurrentHealth += healDone;
            eventHealed?.Invoke(healDone);
        }
    }
}