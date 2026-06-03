using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    /// <summary>
    /// Chef d'orchestre (Bootstrapper).
    /// Gère l'ordre d'initialisation manuel et l'injection par méthode (Init).
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private PoolManager _poolManager;
        [SerializeField] private EnemyFactory _enemyFactory;

        private void Start()
        {
            // Injection explicite par méthode : l'ordre est parfaitement maîtrisé
            if (_enemyFactory != null && _poolManager != null)
            {
                _enemyFactory.Init(_poolManager);
            }
            else
            {
                Debug.LogWarning("GameBootstrapper: Il manque des références pour initialiser le jeu.");
            }
        }
    }
}
