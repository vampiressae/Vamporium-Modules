using UnityEngine;

namespace VamporiumState.SO
{
    [CreateAssetMenu(menuName = "Vamporium/State/States/Log")]
    public class StateLog : State
    {
        [SerializeField] private string _logEnter, _logExit;

        public override void Enter(StateMachine machine)
        {
            base.Enter(machine);
            Debug.Log(string.IsNullOrEmpty(_logEnter) ? "Enter Log:" + GetType().Name : _logEnter);
        }

        public override void Exit(StateMachine machine)
        {
            base.Exit(machine);
            Debug.Log(string.IsNullOrEmpty(_logExit) ? "Exit Log:" + GetType().Name : _logExit);
        }
    }
}
