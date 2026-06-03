using TinyRPG.Core;
using TinyRPG.Data;
using TinyRPG.Stats;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Composant principal d'un ennemi.
    /// Assemble la StateMachine, l'Alterable Stat, et le Flyweight Data.
    /// Implémente IPoolable pour être géré par l'Object Pool.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        public EnemyData Data { get; private set; }
        
        public StateMachine StateMachine { get; private set; }
        public Stat CurrentSpeed { get; private set; }
        
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnDie += HandleDeath;
            
            StateMachine = new StateMachine();
        }

        public void Initialize(EnemyData data)
        {
            Data = data;
            
            // Pattern Alterable (Stats)
            CurrentSpeed = new Stat(Data.BaseMovementSpeed);
            
            _health.Initialize(Data.BaseHealth);
        }

        private void Update()
        {
            StateMachine.Tick();
        }

        private void HandleDeath()
        {
            StateMachine.ChangeState(new EnemyDieState(this));
        }

        public void OnSpawned()
        {
            // Reset state
            StateMachine.ChangeState(new EnemySpawnState(this));
        }

        public void OnDespawned()
        {
            // Nettoyage avant le retour dans la pool
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnDie -= HandleDeath;
            }
        }
    }
}
