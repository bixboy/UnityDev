using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Gère les dégâts sur la durée pour une zone, avec une sécurité 
    /// pour infliger les dégâts instantanément à l'entrée.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class AuraDamageZone : MonoBehaviour
    {
        public float Damage;
        public float TickRate; // Temps en secondes entre deux tics de dégâts

        // Garde en mémoire le dernier moment où un ennemi a pris des dégâts
        private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

        private void OnTriggerEnter(Collider other)
        {
            // SÉCURITÉ : Dès qu'un ennemi entre, il prend des dégâts instantanément.
            // Cela empêche un ennemi très rapide de traverser la zone entre deux "ticks".
            ApplyDamage(other);
        }

        private void OnTriggerStay(Collider other)
        {
            // S'il reste dedans, on vérifie si le délai (TickRate) est écoulé
            if (_lastDamageTime.TryGetValue(other, out float lastTime))
            {
                if (Time.time >= lastTime + TickRate)
                {
                    ApplyDamage(other);
                }
            }
            else
            {
                // Fallback au cas où
                ApplyDamage(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Nettoyage de la mémoire quand l'ennemi sort ou meurt
            if (_lastDamageTime.ContainsKey(other))
            {
                _lastDamageTime.Remove(other);
            }
        }

        private void ApplyDamage(Collider other)
        {
            HitboxProxy hitbox = other.GetComponent<HitboxProxy>();
            if (hitbox != null)
            {
                hitbox.TakeDamage(Damage);
                _lastDamageTime[other] = Time.time;
            }
        }
    }
}
