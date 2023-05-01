using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class EnemyRegen : BaseGameRunner
{

    [field:SerializeField]
    public StatsHolder Stats { get; private set; }

    [field: SerializeField]
    public float RegenDelayInSeconds { get; private set; }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        yield return TimeYields.WaitSeconds(GameTimer, RegenDelayInSeconds);

        var stats = Stats.GetStats();
        stats.Ints[Constants.StatNames.HP] = stats.Ints[Constants.StatNames.MaxHP];
    }
}
