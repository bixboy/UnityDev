using UnityEngine;

namespace TinyRPG.Core
{
    /// <summary>
    /// Pattern State Machine.
    /// Gère les transitions et l'exécution de l'état courant.
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Tick()
        {
            _currentState?.Tick();
        }
    }
}
