using UnityEngine;

namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class HitboxProxy : MonoBehaviour
    {
        [SerializeField] private Health _healthTarget;
        
        public Health HealthTarget => _healthTarget;
        

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
