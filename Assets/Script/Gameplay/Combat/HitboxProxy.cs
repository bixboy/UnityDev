using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Pattern Proxy (Redirection).
    /// Se place sur l'objet contenant le Collider, et redirige
    /// l'appel des collisions/dégâts vers le composant Health principal.
    /// Évite les GetComponentInParent coûteux dans l'arbre.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HitboxProxy : MonoBehaviour
    {
        [SerializeField] private Health _healthTarget;

        public void TakeDamage(float amount)
        {
            if (_healthTarget != null)
            {
                _healthTarget.TakeDamage(amount);
            }
            else
            {
                Debug.LogWarning($"HitboxProxy sur {gameObject.name} n'a pas de cible Health assignée.");
            }
        }
    }
}
