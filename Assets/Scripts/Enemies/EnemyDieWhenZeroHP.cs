using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Builders;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDieWhenZeroHP : BaseGameObject
{
    [field: SerializeField]
    public Enemy Enemy { get; private set; }

    private PlayerIdentifier _player;
    
    [field:SerializeField]
    public AudioSource Death { get; private set; }

    private SFXManager _sfxManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene(true);
        _sfxManager = _sfxManager.FromScene();
    }

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

        if (Death != null)
        {
            _sfxManager.GenericAudioSource.pitch = 0.8f + Random.value * 0.4f;
            _sfxManager.GenericAudioSource.PlayOneShot(Death.clip);
        }

        var stats = _player.PlayerStats.GetStats();
        stats.Ints[Constants.StatNames.Experience] += Enemy.CurrentStats.Ints[Constants.StatNames.Experience];

        DefaultMachinery.AddBasicMachine(TimeWarp());
    }

    private IEnumerable<IEnumerable<Action>> TimeWarp()
    {
        yield return new LerpBuilder(f => GameTimer.Multiplier = f, () => GameTimer.Multiplier)
            .SetTarget(0.01f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Over(0.2f)
            .UsingTimer(UITimer)
            .Build();

        yield return new LerpBuilder(f => GameTimer.Multiplier = f, () => GameTimer.Multiplier)
            .SetTarget(1)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Over(0.3f)
            .UsingTimer(UITimer)
            .Build();
    }
}
