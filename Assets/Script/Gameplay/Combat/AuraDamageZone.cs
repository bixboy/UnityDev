using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class AuraDamageZone : MonoBehaviour
    {
        [HideInInspector] public GameObject Owner;

        public float Damage;
        public float TickRate;

        private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();


        private void OnTriggerEnter(Collider other)
        {
            ApplyDamage(other);
        }


        private void OnTriggerStay(Collider other)
        {
            if (_lastDamageTime.TryGetValue(other, out float lastTime))
            {
                if (Time.time >= lastTime + TickRate)
                {
                    ApplyDamage(other);
                }
            }
            else
            {
                ApplyDamage(other);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (_lastDamageTime.ContainsKey(other))
            {
                _lastDamageTime.Remove(other);
            }
        }



        private void ApplyDamage(Collider other)
        {
            HitboxProxy hitbox = other.GetComponent<HitboxProxy>();
            if (hitbox != null && hitbox.HealthTarget != null)
            {
                if (Owner != null && hitbox.HealthTarget.gameObject == Owner)
                    return;

                hitbox.TakeDamage(Damage);
                _lastDamageTime[other] = Time.time;
            }
        }
    }
}
