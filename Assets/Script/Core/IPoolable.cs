namespace TinyRPG.Core
{

    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}
