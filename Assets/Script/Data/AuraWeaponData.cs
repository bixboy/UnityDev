using UnityEngine;
using TinyRPG.Core;
using TinyRPG.Gameplay;

namespace TinyRPG.Data
{

    [CreateAssetMenu(fileName = "AuraWeapon", menuName = "TinyRPG/Weapons/Aura Zone")]
    public class AuraWeaponData : WeaponData
    {
        public float AuraRadius = 3f;
        
        [Tooltip("Le prefab de l'Aura à faire apparaître (ID de la pool)")]
        public string AuraPoolId;
        

        public override void Fire(Transform playerTransform, PoolManager pool)
        {
            if (pool == null || string.IsNullOrEmpty(AuraPoolId))
                return;

            AuraDamageZone existingAura = playerTransform.GetComponentInChildren<AuraDamageZone>();
            
            if (existingAura == null)
            {
                GameObject auraObj = pool.Spawn(AuraPoolId, playerTransform.position + Vector3.up * 0.1f, Quaternion.identity);
                if (auraObj != null)
                {
                    auraObj.transform.SetParent(playerTransform);
                    auraObj.transform.localScale = new Vector3(AuraRadius * 2f, auraObj.transform.localScale.y, AuraRadius * 2f);

                    AuraDamageZone damageZone = auraObj.GetComponent<AuraDamageZone>();
                    if (damageZone == null)
                    {
                        damageZone = auraObj.AddComponent<AuraDamageZone>();
                    }

                    damageZone.Damage = BaseDamage;
                    damageZone.TickRate = BaseFireRate > 0 ? 1f / BaseFireRate : 1f;
                    
                    Collider col = auraObj.GetComponent<Collider>();
                    if (col != null) col.isTrigger = true;
                }
            }
            else
            {
                existingAura.Damage = BaseDamage;
                existingAura.TickRate = BaseFireRate > 0 ? 1f / BaseFireRate : 1f;
                existingAura.transform.localScale = new Vector3(AuraRadius * 2f, existingAura.transform.localScale.y, AuraRadius * 2f);
            }
        }
    }
}
