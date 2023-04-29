using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class EnemyStatsHolder : StatsHolder
{
    [field:SerializeField]
    public Enemy Enemy { get; private set; }
    public override ObjectStats GetStats()
    {
        return Enemy.CurrentStats;
    }
}
