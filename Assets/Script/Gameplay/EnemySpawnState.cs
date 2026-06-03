using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    public class EnemySpawnState : IState
    {
        private Enemy _enemy;
        private float _spawnDuration = 1.0f;
        private float _timer = 0f;

        public EnemySpawnState(Enemy enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            _timer = 0f;
            // TODO: Jouer animation de Spawn
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnDuration)
            {
                // Fin du spawn, on passe à la chasse
                _enemy.StateMachine.ChangeState(new EnemyChaseState(_enemy));
            }
        }

        public void Exit()
        {
            // Fin de l'invulnérabilité
        }
    }
}
