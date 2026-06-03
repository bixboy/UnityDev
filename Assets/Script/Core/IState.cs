namespace TinyRPG.Core
{
    /// <summary>
    /// Pattern State Machine.
    /// Interface commune pour tous les états d'une machine à états.
    /// </summary>
    public interface IState
    {
        void Enter();
        void Tick();
        void Exit();
    }
}
