using TinyRPG.Core;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    public class EnemyChaseState : IState
    {
        private Enemy _enemy;
        private Transform _playerTarget;


        public EnemyChaseState(Enemy enemy)
        {
            _enemy = enemy;
        }


        public void Enter()
        {
            Player player = ServiceLocator.Get<Player>();
            if (player != null)
            {
                _playerTarget = player.transform;
            }
        }


        public void Tick()
        {
            if (_playerTarget != null && _enemy != null)
            {
                float distance = Vector3.Distance(_playerTarget.position, _enemy.transform.position);

                // Si à portée d'attaque...
                if (distance <= _enemy.Data.AttackRange)
                {
                    if (Time.time >= _enemy.LastAttackTime + _enemy.Data.AttackCooldown)
                    {
                        _enemy.StateMachine.ChangeState(new EnemyAttackState(_enemy, _playerTarget.gameObject));
                    }
                    
                    Vector3 dirToLook = (_playerTarget.position - _enemy.transform.position).normalized;
                    dirToLook.y = 0;
                    if (dirToLook != Vector3.zero)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(dirToLook);
                        _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRot, 10f * Time.deltaTime);
                    }
                    
                    return;
                }

                Vector3 direction = (_playerTarget.position - _enemy.transform.position).normalized;
                direction.y = 0;
                
                // Déplacement
                _enemy.transform.position += direction * _enemy.CurrentSpeed.Value * Time.deltaTime;

                // Rotation vers le joueur
                if (direction != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(direction);
                    _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRot, 10f * Time.deltaTime);
                }
            }
        }


        public void Exit()
        {
        }
    }
}
