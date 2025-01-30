using UnityEngine;

namespace VamporiumStats
{
    [CreateAssetMenu(menuName = "Vamporium/Stats/Effects/Spawn With Prefab")]
    public class SpawnWithPrefab : StatEffectAsset
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _attached;

        public override NeededFields Needed => NeededFields.GameObject;
        public override string GetNeededName(NeededFields field) => field switch
        {
            NeededFields.GameObject => "Parent",
            _ => string.Empty,
        };

        public override void Trigger(StatEffect effect, Stat stat)
        {
            Transform parent = effect.GO.transform;
            var spawn = Instantiate(_prefab, parent.position, parent.rotation, parent);
            if (spawn.TryGetComponent<StatSpawn>(out var component)) component.Init(stat);
        }
    }
}
