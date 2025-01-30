using UnityEngine;

namespace VamporiumState.SO
{
    public abstract class State : ScriptableObject, IState
    {
        public string Name => name;

        public virtual bool CanEnter(StateMachine machine) => true;
        public virtual bool CanExit(StateMachine machine) => true;

        public virtual void Enter(StateMachine machine) { }
        public virtual void Exit(StateMachine machine) { }

        public virtual void UpdateLogics(StateMachine machine) { }
        public virtual void UpdatePhyics(StateMachine machine) { }
    }
}
