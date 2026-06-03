namespace TinyRPG.Stats
{
    public enum StatModType
    {
        Flat = 100, // Addition directe (+10)
        PercentAdd = 200, // +10% 
        PercentMult = 300 // *1.1
    }

    /// <summary>
    /// Pattern Alterable (Transformateur de stat).
    /// Modificateur appliqué à une Stat.
    /// </summary>
    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly object Source;

        public StatModifier(float value, StatModType type, object source = null)
        {
            Value = value;
            Type = type;
            Source = source;
        }
    }
}
