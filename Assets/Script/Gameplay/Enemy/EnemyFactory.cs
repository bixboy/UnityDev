using TinyRPG.Core;
using TinyRPG.Data;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    public class EnemyFactory : MonoBehaviour
    {
        private PoolManager _poolManager;


        public void Init(PoolManager poolManager)
        {
            _poolManager = poolManager;
        }


        public Enemy CreateEnemy(EnemyData data, Vector3 position, Quaternion rotation)
        {
            if (_poolManager == null)
            {
                Debug.LogError("EnemyFactory: PoolManager introuvable via ServiceLocator.");
                return null;
            }

            GameObject enemyObj = _poolManager.Spawn(data.EnemyId, position, rotation);
            
            if (enemyObj != null)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Initialize(data);
                    return enemy;
                }
            }

            return null;
        }
    }
}
