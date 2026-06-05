using TinyRPG.Core;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    public class EnemyAttackState : IState
    {
        private Enemy _enemy;
        private Health _targetHealth;
        private HitboxProxy _targetProxy;
        private float _timer;
        private bool _hasAttacked;


        public EnemyAttackState(Enemy enemy, GameObject target)
        {
            _enemy = enemy;
            
            if (target != null)
            {
                _targetProxy = target.GetComponentInChildren<HitboxProxy>();
                if (_targetProxy == null)
                {
                    _targetHealth = target.GetComponent<Health>();
                }
            }
        }


        public void Enter()
        {
            _timer = 0f;
            _hasAttacked = false;
        }


        public void Tick()
        {
            _timer += Time.deltaTime;

            if (!_hasAttacked && _timer >= 0.1f)
            {
                PerformAttack();
                _enemy.LastAttackTime = Time.time;
            }

            if (_timer >= 0.3f)
            {
                _enemy.StateMachine.ChangeState(new EnemyChaseState(_enemy));
            }
        }


        private void PerformAttack()
        {
            _hasAttacked = true;

            if (_targetProxy != null)
            {
                _targetProxy.TakeDamage(_enemy.Data.BaseDamage);
            }
            else if (_targetHealth != null)
            {
                _targetHealth.TakeDamage(_enemy.Data.BaseDamage);
            }
        }
        

        public void Exit()
        {
        }
    }
}
