using UnityEngine;
using TinyRPG.Core;


namespace TinyRPG.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        public string WeaponName;
        public float BaseDamage;
        public float BaseFireRate;

        public abstract void Fire(Transform playerTransform, PoolManager pool);
    }
}
