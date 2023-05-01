using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class PlayerLevel : BaseGameObject
{
    [field:SerializeField]
    public StatsHolder PlayerStats { get; private set; }

    private ObjectStats _stats;

    [field: SerializeField]
    public int[] ExperienceTable { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        _stats = PlayerStats.GetStats();

        _stats.Ints.GetStat(Constants.StatNames.Experience).OnChange += Experience_OnChange;
    }

    private void Experience_OnChange(ScriptStat<int>.StatUpdate obj)
    {
        var level = _stats.Ints[Constants.StatNames.Level];
        if (level > ExperienceTable.Length) return;

        var required = ExperienceTable[level-1];
        if (obj.NewValue >= required)
        {
            _stats.Ints[Constants.StatNames.Level] = level + 1;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _stats.Ints.GetStat(Constants.StatNames.Experience).OnChange -= Experience_OnChange;
    }
}
