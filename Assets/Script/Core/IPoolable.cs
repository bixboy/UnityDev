namespace TinyRPG.Core
{
    /// <summary>
    /// Interface requise pour tous les objets gérés par la Pool.
    /// </summary>
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}
