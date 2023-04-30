using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class EnemyDieWhenZeroHP : BaseGameObject
{
    [field: SerializeField]
    public Enemy Enemy { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        DefaultMachinery.AddBasicMachine(DelayedEnable());
    }

    public event Action OnDeath;

    private IEnumerable<IEnumerable<Action>> DelayedEnable()
    {
        yield return TimeYields.WaitOneFrameX;

        var stats = Enemy.CurrentStats;
        if (stats == null) yield break;

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange += OnEnemyHPChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        var stats = Enemy.CurrentStats;
        if (stats == null) return;

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange -= OnEnemyHPChange;
    }

    private void OnEnemyHPChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        if (obj.NewValue > 0) return;

        OnDeath?.Invoke();
        Enemy.EndEffect();
    }
}
