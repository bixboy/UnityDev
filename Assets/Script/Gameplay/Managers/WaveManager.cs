using TinyRPG.Core;
using TinyRPG.Data;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Ennemi à spawner")]
        private EnemyData _enemyToSpawn;

        [SerializeField, Tooltip("Intervalle de spawn des ennemis")]
        private float _spawnInterval = 1f;

        [SerializeField, Tooltip("Marge hors caméra (ex: 0.1 = 10% de l'écran)")]
        private float _cameraOverscan = 0.1f;

        [SerializeField] private EnemyFactory _factory;
        private float _timer;


        private void Update()
        {
            if (_factory == null || _enemyToSpawn == null)
                return;

            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval)
            {
                _timer = 0f;
                SpawnEnemy();
            }
        }


        private void SpawnEnemy()
        {
            Vector3 spawnPos = GetSpawnPositionOutsideCamera();
            _factory.CreateEnemy(_enemyToSpawn, spawnPos, Quaternion.identity);
        }


        private Vector3 GetSpawnPositionOutsideCamera()
        {
            Player player = ServiceLocator.Get<Player>();
            if (player == null || Camera.main == null)
                return transform.position;

            // Choisit un côté de l'écran (0: Haut, 1: Bas, 2: Gauche, 3: Droite)
            int side = Random.Range(0, 4);
            Vector2 viewportPos = Vector2.zero;

            switch (side)
            {
                case 0:
                    viewportPos = new Vector2(Random.Range(-_cameraOverscan, 1f + _cameraOverscan), 1f + _cameraOverscan);
                    break; // Haut

                case 1:
                    viewportPos = new Vector2(Random.Range(-_cameraOverscan, 1f + _cameraOverscan), -_cameraOverscan);
                    break; // Bas

                case 2:
                    viewportPos = new Vector2(-_cameraOverscan, Random.Range(-_cameraOverscan, 1f + _cameraOverscan));
                    break; // Gauche

                case 3:
                    viewportPos = new Vector2(1f + _cameraOverscan, Random.Range(-_cameraOverscan, 1f + _cameraOverscan));
                    break; // Droite
            }

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(viewportPos.x, viewportPos.y, 0f));
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, player.transform.position.y, 0));

            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return player.transform.position + new Vector3(15f, 0f, 15f); 
        }
    }
}
