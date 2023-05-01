using System;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class UI_GlobeMask : BaseGameObject
{
    [field:SerializeField]
    public string Stat { get; private set; }

    [field: SerializeField]
    public string MaxStat { get; private set; }
    [field: SerializeField]
    public Vector3 FullLocalPosition { get; private set; }
    [field: SerializeField]
    public Vector3 EmptyLocalPosition { get; private set; }

    private PlayerIdentifier _player;
    private ObjectStats _stats;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _stats = _player.PlayerStats.GetStats();
        _stats.Ints.GetStat(Stat).OnChange += OnStatChange;
        Recalculate(_stats.Ints[Stat]);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _stats.Ints.GetStat(Stat).OnChange -= OnStatChange;
    }

    private void OnStatChange(ScriptStat<int>.StatUpdate obj)
    {
        Recalculate(obj.NewValue);
    }

    private void Recalculate(int statValue)
    {
        var proportion = statValue / (float)_stats.Ints[MaxStat];
        transform.localPosition = Vector3.Lerp(EmptyLocalPosition, FullLocalPosition, proportion);
    }
}
