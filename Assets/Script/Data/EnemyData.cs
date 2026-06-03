using UnityEngine;

namespace TinyRPG.Data
{
    /// <summary>
    /// Pattern SOAP (Scriptable Object Architecture) / Flyweight.
    /// Contient les données invariables des ennemis pour ne pas 
    /// les dupliquer sur chaque instance dans la scène.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "TinyRPG/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string EnemyId;
        public float BaseHealth;
        public float BaseMovementSpeed;
        public float BaseDamage;
        public int ExpReward;
    }
}
