using UnityEngine;
using TinyRPG.Core;
using TinyRPG.Gameplay;


namespace TinyRPG.Data
{

    [CreateAssetMenu(fileName = "ForwardWeapon", menuName = "TinyRPG/Weapons/Forward Projectile")]
    public class ForwardWeaponData : WeaponData
    {
        public float ProjectileSpeed = 15f;
        public string ProjectilePoolId;


        public override void Fire(Transform playerTransform, PoolManager pool)
        {
            if (pool == null)
                return;

            Vector3 spawnPos = playerTransform.position + Vector3.up * 1f;
            GameObject projObj = pool.Spawn(ProjectilePoolId, spawnPos, playerTransform.rotation);
                
            if (projObj != null)
            {
                Projectile projectile = projObj.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Speed = ProjectileSpeed;
                    projectile.Damage = BaseDamage;
                    projectile.PoolId = ProjectilePoolId;
                }
            }
        }
    }
}
