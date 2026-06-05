using UnityEngine;


namespace TinyRPG.Data
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "TinyRPG/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string EnemyId;
        public float BaseHealth;
        public float BaseMovementSpeed;
        public float BaseDamage;
        public float AttackRange = 1.5f;
        public float AttackCooldown = 1.0f;
        public int ExpReward;
    }
}
