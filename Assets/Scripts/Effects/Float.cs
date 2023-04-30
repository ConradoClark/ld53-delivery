using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class Float : BaseGameRunner
{
    [field:SerializeField]
    public Vector3 TargetPosition { get; private set; }

    [field: SerializeField]
    public float FrequencyInSeconds { get; private set; }

    private Vector3 _originalPosition;
    protected override void OnAwake()
    {
        base.OnAwake();
        _originalPosition = transform.localPosition;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        yield return transform.GetAccessor()
            .LocalPosition
            .ToPosition(TargetPosition)
            .Over(FrequencyInSeconds * 0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        transform.localPosition = TargetPosition;

        yield return transform.GetAccessor()
            .LocalPosition
            .ToPosition(_originalPosition)
            .Over(FrequencyInSeconds * 0.5f)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .UsingTimer(GameTimer)
            .Build();

        transform.localPosition = _originalPosition;
    }
}
