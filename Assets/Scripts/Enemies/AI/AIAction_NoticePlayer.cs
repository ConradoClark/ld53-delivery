using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using Licht.Unity.UI;
using UnityEngine;

public class AIAction_NoticePlayer : BaseAIAction
{
    [field:SerializeField]
    public ScriptPrefab ExclamationPrefab { get; private set; }
    private UIGenericPool _exclamationPool;

    [field:SerializeField]
    public Vector3 EffectOffset { get; private set; }

    private Camera _mainCamera;
    private Camera _uiCamera;

    [field:SerializeField]
    public float NoticeDelayInMs { get; private set; }

    private PooledComponent _currentExclamation;

    protected override void OnAwake()
    {
        base.OnAwake();
        _exclamationPool = SceneObject<UIGenericPoolManager>.Instance().GetEffect(ExclamationPrefab);
        _mainCamera = Camera.main;
        _uiCamera = SceneObject<UICamera>.Instance().Camera;
    }

    public override IEnumerable<IEnumerable<Action>> Execute(Func<bool> breakCondition)
    {
        _exclamationPool.TryGetFromPool(out _currentExclamation, effect =>
        {
            var pos = transform.position + EffectOffset;
            effect.transform.position = _uiCamera.ViewportToWorldPoint(_mainCamera.WorldToViewportPoint(pos));
        });
        yield return TimeYields.WaitMilliseconds(GameTimer, NoticeDelayInMs, breakCondition: breakCondition);
    }

    public override void OnInterrupt()
    {
        if (_currentExclamation == null) return;
        _currentExclamation.EndEffect();;
    }
}
