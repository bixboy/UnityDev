using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinyRPG.Stats
{
    /// <summary>
    /// Pattern Alterable (Pipeline de Stats).
    /// Gère une valeur de base et calcule sa valeur finale à travers 
    /// une liste de transformateurs (StatModifier).
    /// Utilise le concept de "Dirty Flag" pour n'actualiser la stat que si nécessaire.
    /// </summary>
    [Serializable]
    public class Stat
    {
        public float BaseValue;

        private bool _isDirty = true;
        private float _lastCalculatedValue;

        private readonly List<StatModifier> _modifiers;

        public Stat(float baseValue)
        {
            BaseValue = baseValue;
            _modifiers = new List<StatModifier>();
        }

        public float Value
        {
            get
            {
                if (_isDirty)
                {
                    _lastCalculatedValue = CalculateFinalValue();
                    _isDirty = false;
                }
                return _lastCalculatedValue;
            }
        }

        public void AddModifier(StatModifier mod)
        {
            _isDirty = true;
            _modifiers.Add(mod);
            // On trie la liste pour garantir l'ordre d'application (Flat -> PercentAdd -> PercentMult)
            _modifiers.Sort((a, b) => a.Type.CompareTo(b.Type));
        }

        public bool RemoveModifier(StatModifier mod)
        {
            if (_modifiers.Remove(mod))
            {
                _isDirty = true;
                return true;
            }
            return false;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].Source == source)
                {
                    _isDirty = true;
                    didRemove = true;
                    _modifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                StatModifier mod = _modifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    // Cumule les pourcentages additifs avant application (+10% et +20% font +30%)
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= _modifiers.Count || _modifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    // Multiplicateurs indépendants (* 1.1)
                    finalValue *= 1 + mod.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}
