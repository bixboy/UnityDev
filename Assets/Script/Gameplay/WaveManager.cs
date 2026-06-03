using TinyRPG.Core;
using TinyRPG.Data;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyToSpawn;
        [SerializeField] private float _spawnInterval = 1f;
        [SerializeField] private float _spawnRadius = 15f;
        
        [SerializeField] private EnemyFactory _factory;
        private float _timer;

        private void Update()
        {
            if (_factory == null || _enemyToSpawn == null) return;

            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval)
            {
                _timer = 0f;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            // Calcule une position aléatoire autour du spawner (ou du joueur)
            Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // On utilise la Factory (Pattern Factory Method)
            _factory.CreateEnemy(_enemyToSpawn, spawnPos, Quaternion.identity);
        }
    }
}
