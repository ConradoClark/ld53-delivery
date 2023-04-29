using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

[RequireComponent(typeof(RoomObject))]
public class RoomExit : BaseGameObject
{
    [field:SerializeField]
    public RoomSpawn TargetSpawn { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    public RoomObject RoomObject { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        RoomObject = GetComponent<RoomObject>();
        PhysicsObject.AddCustomObject(this);
    }

    private void OnDrawGizmos()
    {
        var checkRefRoom = RoomObject == null || !RoomObject.Room.IsActive;
        if (checkRefRoom && Application.isFocused) return;

        var pos = Application.isFocused ? transform.localPosition : transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.25f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(pos, 0.3f);
    }

    private void OnDrawGizmosSelected()
    {
        if (TargetSpawn == null) return;

        var pos = Application.isFocused ? transform.localPosition : transform.position;
        var target = Application.isFocused ? TargetSpawn.transform.localPosition : TargetSpawn.transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, target);
    }

    public bool CanExit()
    {
        return !TargetSpawn.Spawning;
    }

    public IEnumerable<IEnumerable<Action>> Exit()
    {
        if (!CanExit()) yield break;
        yield return TargetSpawn.Spawn(RoomObject.Room).AsCoroutine();
    }
}
