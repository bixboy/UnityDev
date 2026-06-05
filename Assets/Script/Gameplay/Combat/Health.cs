using System;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Composant de Santé.
    /// Point central qui reçoit les dégâts de toutes sources.
    /// </summary>
    public class Health : MonoBehaviour
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public event Action<float> OnTakeDamage;
        public event Action OnDie;

        public void Initialize(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            OnTakeDamage?.Invoke(amount);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            CurrentHealth = 0;
            OnDie?.Invoke();
        }
    }
}
