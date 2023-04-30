using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;
using UnityEngine.InputSystem;

public class Waypoint : BaseGameRunner
{
    [field:SerializeField]
    public InputActionReference ConfirmInput { get; private set; }
    [field: SerializeField]
    public LichtPhysicsCollisionDetector CollisionDetector { get; private set; }

    private WaypointManager _waypointManager;
    public RoomObject RoomObject { get; private set; }
    
    protected override void OnAwake()
    {
        base.OnAwake();
        RoomObject = GetComponent<RoomObject>();
        _waypointManager = _waypointManager.FromScene();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (CollisionDetector.Triggers.Any(t => t.TriggeredHit) 
            && ConfirmInput.action.WasPerformedThisFrame())
        {
            _waypointManager.CurrentWaypoint = this;
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
