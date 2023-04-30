using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class UI_DialogueBar : BaseGameObject
{
    [field: SerializeField]
    public Vector3 InitialPosition { get; private set; }
    [field: SerializeField]
    public Vector3 TargetPosition { get; private set; }
    [field: SerializeField]
    public float AnimDurationInSeconds { get; private set; }

    private UI_Dialogue _dialogue;
    private PlayerIdentifier _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _dialogue = _dialogue.FromScene();
        _player = _player.FromScene();
    }

    public IEnumerable<IEnumerable<Action>> Show(params string[] text)
    {
        _player.MoveController.BlockMovement(this);
        transform.localPosition = InitialPosition;

        _dialogue.Clear();

        yield return transform.GetAccessor()
            .LocalPosition
            .ToPosition(TargetPosition)
            .Over(AnimDurationInSeconds)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseInOut)
            .Build();

        foreach (var piece in text)
        {
            yield return _dialogue.ShowText(piece).AsCoroutine();
        }

        _player.MoveController.UnblockMovement(this);
        yield return Hide().AsCoroutine();
    }

    public IEnumerable<IEnumerable<Action>> Hide()
    {
        _dialogue.Clear();
        transform.localPosition = TargetPosition;

        yield return transform.GetAccessor()
            .LocalPosition
            .ToPosition(InitialPosition)
            .Over(AnimDurationInSeconds)
            .UsingTimer(GameTimer)
            .Easing(EasingYields.EasingFunction.QuadraticEaseIn)
            .Build();
    }
}
