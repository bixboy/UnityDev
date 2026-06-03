using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG.Core
{
    /// <summary>
    /// Implémentation du pattern Service Locator.
    /// Évite l'utilisation excessive de Singletons stricts (anti-pattern)
    /// en fournissant un registre global de services découplés.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services.Add(type, service);
            }
            else
            {
                Debug.LogWarning($"ServiceLocator: Un service de type {type} est déjà enregistré. Écrasement.");
                _services[type] = service;
            }
        }

        public static void Unregister<T>()
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                _services.Remove(type);
            }
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            Debug.LogError($"ServiceLocator: Aucun service de type {type} n'a été enregistré.");
            return default;
        }
    }
}
