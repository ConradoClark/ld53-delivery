using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class UI_ScreenFlash : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field:SerializeField]
    public float FadeTimeInSeconds { get; private set; }

    public IEnumerable<IEnumerable<Action>> FadeIn()
    {
        SpriteRenderer.enabled = true;
        yield return SpriteRenderer.FadeIn(FadeTimeInSeconds)
            .Easing(EasingYields.EasingFunction.QuadraticEaseOut)
            .Build();
    }

    public IEnumerable<IEnumerable<Action>> FadeOut()
    {
        SpriteRenderer.enabled = true;
        yield return SpriteRenderer.FadeOut(FadeTimeInSeconds)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
        SpriteRenderer.enabled = false;
    }
}
