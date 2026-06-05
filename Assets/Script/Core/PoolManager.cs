using System.Collections.Generic;
using UnityEngine;


namespace TinyRPG.Core
{
    public class PoolManager : MonoBehaviour
    {

        [System.Serializable]
        public struct PoolConfig
        {
            [Tooltip("Identifiant unique de la pool (ex: 'EnemyOrc', 'Fireball')")]
            public string PoolId;
            
            [Tooltip("Le Prefab à instancier en masse")]
            public GameObject Prefab;
            
            [Tooltip("Nombre d'objets à créer au tout début du jeu (mis en cache)")]
            public int InitialSize;
        }


        [SerializeField] private List<PoolConfig> _poolConfigs;
        
        private Dictionary<string, Queue<GameObject>> _pools;
        private Dictionary<string, GameObject> _prefabs;
        

        private void Awake()
        {
            ServiceLocator.Register<PoolManager>(this);
            InitializePools();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<PoolManager>();
        }


        private void InitializePools()
        {
            _pools = new Dictionary<string, Queue<GameObject>>();
            _prefabs = new Dictionary<string, GameObject>();

            foreach (var config in _poolConfigs)
            {
                _pools[config.PoolId] = new Queue<GameObject>();
                _prefabs[config.PoolId] = config.Prefab;

                GameObject poolParent = new GameObject($"Pool_{config.PoolId}");
                poolParent.transform.SetParent(this.transform);

                for (int i = 0; i < config.InitialSize; i++)
                {
                    GameObject obj = Instantiate(config.Prefab, poolParent.transform);

                    obj.SetActive(false);

                    _pools[config.PoolId].Enqueue(obj);
                }
            }
        }


        public GameObject Spawn(string poolId, Vector3 position, Quaternion rotation)
        {
            if (!_pools.ContainsKey(poolId))
            {
                Debug.LogError($"PoolManager: Erreur critique, la pool '{poolId}' n'existe pas dans l'inspecteur.");
                return null;
            }

            GameObject obj;
            
            if (_pools[poolId].Count > 0)
            {
                obj = _pools[poolId].Dequeue();
            }
            else
            {
                obj = Instantiate(_prefabs[poolId], transform.Find($"Pool_{poolId}"));
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnSpawned();

            return obj;
        }


        public void Despawn(string poolId, GameObject obj)
        {
            if (!_pools.ContainsKey(poolId))
            {
                Debug.LogError($"PoolManager: Tentative de despawn dans une pool inexistante '{poolId}'. L'objet va être détruit de force.");
                Destroy(obj);
                return;
            }

            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnDespawned();

            Transform poolParent = transform.Find($"Pool_{poolId}");
            if (poolParent != null)
            {
                obj.transform.SetParent(poolParent);
            }

            obj.SetActive(false);
            _pools[poolId].Enqueue(obj);
        }
    }
}
