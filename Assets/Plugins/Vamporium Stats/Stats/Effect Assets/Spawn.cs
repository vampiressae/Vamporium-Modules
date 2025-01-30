using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Spawn")]
    public class Spawn : StatEffectAsset
    {
        public override NeededFields Needed => NeededFields.GameObject | NeededFields.Prefab;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.GameObject => "Parent",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat)
        {
            Transform parent = effect.GO.transform;
            var spawn = Instantiate(effect.Prefab, parent.position, parent.rotation, parent);
            if (spawn.TryGetComponent<StatSpawn>(out var component)) component.Init(stat);
        }
    }
}
