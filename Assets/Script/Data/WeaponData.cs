using UnityEngine;

namespace TinyRPG.Data
{
    /// <summary>
    /// Configuration d'une arme (SOAP).
    /// </summary>
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "TinyRPG/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName;
        public float BaseDamage;
        public float BaseFireRate; // Tir par seconde
        public float ProjectileSpeed;
        public string ProjectilePoolId;
    }
}
