using UnityEngine;

namespace VamporiumState.SO
{
    [CreateAssetMenu(menuName = "Vamporium/State/States/Delay")]
    public class StateDelay : State
    {
        [SerializeField] private float _autoAdvance = 1;

        public override void UpdateLogics(StateMachine machine)
        {
            base.UpdateLogics(machine);
            if (machine.TimeInState < _autoAdvance) return;

            machine.AdvanceState();
        }
    }
}
