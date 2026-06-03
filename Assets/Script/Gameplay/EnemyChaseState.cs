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
            // Récupération de la cible via le ServiceLocator (Player n'a pas encore été créé, on simule)
            // Dans un vrai cas, on aurait ServiceLocator.Get<Player>().transform;
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _playerTarget = playerObj.transform;
            }
        }

        public void Tick()
        {
            if (_playerTarget != null && _enemy != null)
            {
                Vector3 direction = (_playerTarget.position - _enemy.transform.position).normalized;
                // Vitesse dynamique via l'Alterable Stat
                _enemy.transform.position += direction * _enemy.CurrentSpeed.Value * Time.deltaTime;
            }
        }

        public void Exit()
        {
        }
    }
}
