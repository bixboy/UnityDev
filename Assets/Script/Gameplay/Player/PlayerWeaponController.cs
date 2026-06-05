using TinyRPG.Core;
using TinyRPG.Data;
using System.Collections.Generic;
using UnityEngine;


namespace TinyRPG.Gameplay
{

    public class PlayerWeaponController : MonoBehaviour
    {
        [System.Serializable]
        public class WeaponTracker
        {
            public WeaponData Data;
            [HideInInspector] public float Timer;
        }


        [SerializeField, Tooltip("Liste des armes équipées par le joueur")] 
        private List<WeaponTracker> _weapons = new List<WeaponTracker>();


        private void Update()
        {
            foreach (var tracker in _weapons)
            {
                if (tracker.Data == null)
                    continue;

                tracker.Timer += Time.deltaTime;

                float cooldown = 1f / tracker.Data.BaseFireRate;
                
                if (tracker.Timer >= cooldown)
                {
                    tracker.Timer = 0f;
                    PoolManager pool = ServiceLocator.Get<PoolManager>();
                    
                    // STRATEGY PATTERN : Le contrôleur ne sait pas comment l'arme fonctionne,
                    // il se contente de déclencher le tir. C'est l'arme qui fait le travail !
                    tracker.Data.Fire(transform, pool);
                }
            }
        }
    }
}
