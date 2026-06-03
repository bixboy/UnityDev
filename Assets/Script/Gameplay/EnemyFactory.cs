using TinyRPG.Core;
using TinyRPG.Data;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Pattern Factory Method.
    /// Gère la création logique d'un ennemi en abstrayant 
    /// la récupération depuis l'Object Pool et l'initialisation des données (SOAP).
    /// </summary>
    public class EnemyFactory : MonoBehaviour
    {
        private PoolManager _poolManager;

        public void Init(PoolManager poolManager)
        {
            _poolManager = poolManager;
        }


        /// <summary>
        /// Fabrique un ennemi à partir de ses données.
        /// </summary>
        public Enemy CreateEnemy(EnemyData data, Vector3 position, Quaternion rotation)
        {
            if (_poolManager == null)
            {
                Debug.LogError("EnemyFactory: PoolManager introuvable via ServiceLocator.");
                return null;
            }

            // Récupère l'instance depuis la pool
            GameObject enemyObj = _poolManager.Spawn(data.EnemyId, position, rotation);
            
            if (enemyObj != null)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // Initialisation avec le Flyweight (EnemyData)
                    enemy.Initialize(data);
                    return enemy;
                }
            }

            return null;
        }
    }
}
