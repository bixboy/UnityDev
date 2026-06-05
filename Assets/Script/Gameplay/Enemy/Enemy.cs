using TinyRPG.Core;
using TinyRPG.Data;
using TinyRPG.Stats;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour, IPoolable
    {
        public EnemyData Data { get; private set; }
        
        public StateMachine StateMachine { get; private set; }
        public Stat CurrentSpeed { get; private set; }
        public float LastAttackTime { get; set; } = -999f;
        
        private Health _health;


        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnDie += HandleDeath;
            _health.OnTakeDamage += HandleTakeDamage;
            
            StateMachine = new StateMachine();
        }

        private void HandleTakeDamage(float amount)
        {
            PoolManager pool = ServiceLocator.Get<PoolManager>();
            if (pool != null)
            {
                GameObject textObj = pool.Spawn("FloatingText", transform.position + Vector3.up * 2.5f, Quaternion.identity);
                if (textObj != null)
                {
                    TinyRPG.UI.FloatingDamageText floatingText = textObj.GetComponent<TinyRPG.UI.FloatingDamageText>();
                    if (floatingText != null)
                    {
                        floatingText.Setup(amount);
                    }
                }
            }
        }


        public void Initialize(EnemyData data)
        {
            Data = data;
            
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
                _health.OnTakeDamage -= HandleTakeDamage;
            }
        }
    }
}
