using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUp : BaseGameObject
{
    [field: SerializeField]
    public StatsHolder PlayerStats { get; private set; }

    private ObjectStats _stats;

    [field: SerializeField]
    public ScriptPrefab Effect { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        _stats = PlayerStats.GetStats();

        _stats.Ints.GetStat(Constants.StatNames.Level).OnChange += Level_OnChange;
    }

    private void Level_OnChange(ScriptStat<int>.StatUpdate obj)
    {
        Effect.TrySpawnEffect(transform.position, out _);
        var possibleStatus = new[]
        {
            Constants.StatNames.Attack,
            Constants.StatNames.Defense,
            Constants.StatNames.Speed,
            Constants.StatNames.Luck,
            Constants.StatNames.AoE,
        };

        var stat = possibleStatus[Random.Range(0, possibleStatus.Length)];

        _stats.Ints[stat] += 1;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        _stats.Ints.GetStat(Constants.StatNames.Level).OnChange -= Level_OnChange;
    }

}
