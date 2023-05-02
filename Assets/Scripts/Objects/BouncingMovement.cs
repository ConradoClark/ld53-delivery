using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class BouncingMovement : BaseGameRunner
{
    [field:SerializeField]
    public SpriteRenderer PollenSpriteRenderer { get; private set; }

    [field: SerializeField]
    public float MinHeight { get; private set; }

    [field: SerializeField]
    public float MaxHeight { get; private set; }

    [field: SerializeField]
    public float MovementDurationInSeconds { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.localPosition = Vector3.zero;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var height = Random.Range(MinHeight, MaxHeight);

        var heightMovementUp = PollenSpriteRenderer.transform.GetAccessor()
            .LocalPosition
            .Y
            .Increase(height)
            .Over(MovementDurationInSeconds * 0.35f)
            .Easing(EasingYields.EasingFunction.QuarticEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        var heightMovementDown = PollenSpriteRenderer.transform.GetAccessor()
            .LocalPosition
            .Y
            .Decrease(height)
            .Over(MovementDurationInSeconds * 0.35f)
            .Easing(EasingYields.EasingFunction.BounceEaseOut)
            .UsingTimer(GameTimer)
            .Build();

        yield return heightMovementUp.Then(heightMovementDown);
    }
}
