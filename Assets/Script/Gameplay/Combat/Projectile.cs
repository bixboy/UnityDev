using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour, IPoolable
    {
        [HideInInspector] public string PoolId;
        [HideInInspector] public float Speed;
        [HideInInspector] public float Damage;
        public float LifeTime = 3f;
        
        [Tooltip("Vitesse de rotation du projectile sur lui-même (ex: X=0, Y=0, Z=360 pour un shuriken)")]
        public Vector3 SpinSpeed = Vector3.zero;

        private float _timer;
        private Vector3 _direction;

        public void OnSpawned()
        {
            _timer = 0f;
            
            _direction = transform.forward;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        private void Awake()
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        public void OnDespawned()
        {
        }

        private void Update()
        {
            transform.position += _direction * Speed * Time.deltaTime;

            if (SpinSpeed != Vector3.zero)
            {
                transform.Rotate(SpinSpeed * Time.deltaTime, Space.Self);
            }

            _timer += Time.deltaTime;
            if (_timer >= LifeTime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
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
