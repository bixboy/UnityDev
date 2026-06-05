using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    public class EnemyDieState : IState
    {
        private Enemy _enemy;
        private float _dieDuration = 0.5f;
        private float _timer = 0f;

        public EnemyDieState(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _timer = 0f;
            
            // Drop de l'XP
            // Exemple de Factory pour spawner une gemme
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer >= _dieDuration)
            {
                // Remet l'ennemi dans le pool
                PoolManager poolManager = ServiceLocator.Get<PoolManager>();
                if (poolManager != null)
                {
                    poolManager.Despawn(_enemy.Data.EnemyId, _enemy.gameObject);
                }
                else
                {
                    GameObject.Destroy(_enemy.gameObject);
                }
            }
        }

        public void Exit()
        {
        }
    }
}
