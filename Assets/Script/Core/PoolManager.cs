using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG.Core
{
    /// <summary>
    /// Implémentation du pattern Object Pool.
    /// Pré-instancie des GameObjects et les recycle au lieu de les Destroy/Instantiate.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public struct PoolConfig
        {
            public string PoolId;
            public GameObject Prefab;
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

                // Création d'un dossier (GameObject vide) pour ranger la pool dans l'hiérarchie
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
                Debug.LogError($"PoolManager: Pool '{poolId}' n'existe pas.");
                return null;
            }

            GameObject obj;
            if (_pools[poolId].Count > 0)
            {
                obj = _pools[poolId].Dequeue();
            }
            else
            {
                // La pool est vide, on doit agrandir dynamiquement
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
                Debug.LogError($"PoolManager: Tentative de despawn dans une pool inexistante '{poolId}'.");
                Destroy(obj);
                return;
            }

            IPoolable poolable = obj.GetComponent<IPoolable>();
            poolable?.OnDespawned();

            obj.SetActive(false);
            _pools[poolId].Enqueue(obj);
        }
    }
}
