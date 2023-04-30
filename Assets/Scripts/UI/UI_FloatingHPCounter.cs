using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Pooling;
using Licht.Unity.UI;
using UnityEngine;

public class UI_FloatingHPCounter : PooledComponent
{
    [field:SerializeField]
    public SpriteRenderer BorderSpriteRenderer { get; private set; }
    [field: SerializeField]
    public SpriteRenderer CounterSpriteRenderer { get; private set; }

    [field: SerializeField]
    public float InitCounterSize { get; private set; }

    [field: SerializeField]
    public float MaxCounterSize { get; private set; }

    public PooledComponent Source { get; set; }

    public Vector3 PositionOffset { get; set; }

    public StatsHolder StatsHolder { get; set; }
    public ObjectStats Stats { get; set; }

    private Camera _mainCamera;
    private Camera _uiCamera;

    protected override void OnAwake()
    {
        base.OnAwake();
        _mainCamera = Camera.main;
        _uiCamera = SceneObject<UICamera>.Instance().Camera;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        var stats = Stats != null ? Stats : StatsHolder != null ? StatsHolder.GetStats() : null;
        if (stats == null) return;
        stats.Ints.GetStat(Constants.StatNames.HP).OnChange += OnHPChange;
        Source.OnEffectOver += Source_OnEffectOver;
    }

    private void Source_OnEffectOver()
    {
        EndEffect();
    }

    private void OnHPChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        RecalculateHPCounterSize(obj.NewValue);
    }

    private void Update()
    {
        if (Source == null) return;

        if (!Source.ComponentEnabled)
        {
            EndEffect();
            return;
        }
        
        var pos = Source.transform.position + PositionOffset;
        transform.position = _uiCamera.ViewportToWorldPoint(_mainCamera.WorldToViewportPoint(pos));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        var stats = Stats != null ? Stats : StatsHolder != null ? StatsHolder.GetStats() : null;
        if (stats == null) return;
        stats.Ints.GetStat(Constants.StatNames.HP).OnChange -= OnHPChange;
        Source.OnEffectOver -= Source_OnEffectOver;
    }

    private void RecalculateHPCounterSize(int currentHP)
    {
        var stats = Stats != null ? Stats : StatsHolder != null ? StatsHolder.GetStats() : null;
        if (stats == null) return;
        var maxHP = stats.Ints[Constants.StatNames.MaxHP];
        var proportion = currentHP / (float) maxHP;
        CounterSpriteRenderer.size = new Vector2(Mathf.Lerp(InitCounterSize, MaxCounterSize, proportion)
            , CounterSpriteRenderer.size.y);

        DefaultMachinery.AddUniqueMachine($"enemyCounter_${GetInstanceID()}",
            UniqueMachine.UniqueMachineBehaviour.Replace,
            ShowHPTemporarily());
    }

    private IEnumerable<IEnumerable<Action>> ShowHPTemporarily()
    {
        yield return Show().AsCoroutine();
        yield return TimeYields.WaitSeconds(GameTimer, 2);
        yield return Hide().AsCoroutine();
    }

    public IEnumerable<IEnumerable<Action>> Hide()
    {
        BorderSpriteRenderer.enabled = CounterSpriteRenderer.enabled = false;
        yield break;
    }

    public IEnumerable<IEnumerable<Action>> Show()
    {
        BorderSpriteRenderer.enabled = CounterSpriteRenderer.enabled = true;
        yield break;
    }
}
