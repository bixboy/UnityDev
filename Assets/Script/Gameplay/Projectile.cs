using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour, IPoolable
    {
        public string PoolId;
        public float Speed = 10f;
        public float Damage = 10f;
        public float LifeTime = 3f;

        private float _timer;

        public void OnSpawned()
        {
            _timer = 0f;
        }

        public void OnDespawned()
        {
        }

        private void Update()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;

            _timer += Time.deltaTime;
            if (_timer >= LifeTime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Utilisation du Pattern Proxy ! On tape la Hitbox, qui transmettra à Health
            HitboxProxy hitbox = other.GetComponent<HitboxProxy>();
            if (hitbox != null)
            {
                hitbox.TakeDamage(Damage);
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            PoolManager poolManager = ServiceLocator.Get<PoolManager>();
            if (poolManager != null)
            {
                poolManager.Despawn(PoolId, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
