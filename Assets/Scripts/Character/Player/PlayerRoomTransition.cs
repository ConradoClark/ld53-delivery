using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class PlayerRoomTransition : BaseGameRunner
{
    [field:SerializeField]
    public ScriptIdentifier Trigger { get; private set; }

    private PlayerIdentifier _player;
    private LichtPhysics _physics;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _physics = this.GetLichtPhysics();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (_player.PhysicsObject.GetPhysicsTriggerWithSource(Trigger, out var source)
            && source is LichtPhysicsCollisionDetector collisionDetector
            && _physics.TryGetPhysicsObjectByCollider(collisionDetector.Triggers.FirstOrDefault(t => t.TriggeredHit).Collider, out var targetObject)
            && targetObject.TryGetCustomObject<RoomExit>(out var targetExit)
            && targetExit.CanExit())
        {
            Debug.Log($"Exiting through:'{targetExit.gameObject.name}'");
            yield return targetExit.Exit().AsCoroutine();
            yield return TimeYields.WaitMilliseconds(GameTimer, 150);
            yield break;
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
