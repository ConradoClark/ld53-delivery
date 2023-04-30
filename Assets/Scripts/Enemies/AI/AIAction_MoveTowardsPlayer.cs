using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Physics;
using UnityEngine;

public class AIAction_MoveTowardsPlayer : BaseAIAction
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public float TimeInSeconds { get; private set; }

    private PlayerIdentifier _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    public override IEnumerable<IEnumerable<Action>> Execute(Func<bool> breakCondition)
    {
        yield return TimeYields.WaitSeconds(GameTimer, TimeInSeconds, _ =>
        {
            var direction = (_player.transform.position - PhysicsObject.transform.position)
                .normalized;
            if (ComponentEnabled) PhysicsObject.ApplySpeed(direction * Speed);
        }, () => !ComponentEnabled || breakCondition());
    }

    public override void OnInterrupt()
    {
    }
}
