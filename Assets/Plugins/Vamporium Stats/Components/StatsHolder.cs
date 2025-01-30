using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VamporiumStats
{
    public class StatsHolder : MonoBehaviour
    {
        public List<Stat> Stats = new();

        private void Start() => Stats.ForEach(stat => stat.Init());
        private void OnDestroy() => Stats.ForEach(stat => stat.Uninit());

        public bool GetStat(StatAsset asset, out Stat stat)
        {
            stat = Stats.Where(s => s.Asset == asset).FirstOrDefault();
            return stat != null;
        }
    }
}