using Licht.Unity.Objects;
using UnityEngine;

public class EnemyDieWhenZeroHP : BaseGameObject
{
    [field:SerializeField]
    public StatsHolder StatsHolder { get; private set; }

    [field: SerializeField]
    public Enemy Enemy { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        var stats = StatsHolder.GetStats();
        if (stats == null) return;

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange += OnEnemyHPChange;
    }

    private void OnEnemyHPChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        if (obj.NewValue > 0) return;
        Enemy.EndEffect();
    }
}
