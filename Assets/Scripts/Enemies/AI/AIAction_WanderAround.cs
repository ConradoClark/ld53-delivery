using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Physics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIAction_WanderAround : BaseAIAction
{
    [field: SerializeField]
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
        var direction = Random.insideUnitCircle;
        yield return TimeYields.WaitSeconds(GameTimer, TimeInSeconds, _ =>
        {
            if (ComponentEnabled) PhysicsObject.ApplySpeed(direction * Speed);
        }, () => !ComponentEnabled || breakCondition());
    }

    public override void OnInterrupt()
    {
    }
}