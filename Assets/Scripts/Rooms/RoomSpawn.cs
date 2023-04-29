using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

[RequireComponent(typeof(RoomSpawner))]
public class RoomSpawn : BaseGameObject
{
    public bool Spawning { get; private set; }
    private RoomSpawner _roomSpawner;
    private Room _refRoom;

    protected override void OnAwake()
    {
        base.OnAwake();
        _refRoom = GetComponentInParent<Room>();
        _roomSpawner = GetComponent<RoomSpawner>();
    }

    private void OnDrawGizmos()
    {
        var checkRefRoom = _refRoom == null || !_refRoom.IsActive;
        if (checkRefRoom && Application.isFocused) return;

        var pos = Application.isFocused ? transform.localPosition : transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pos, 0.25f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(pos, 0.3f);
    }

    public IEnumerable<IEnumerable<Action>> Spawn(Room sourceRoom)
    {
        Spawning = true;
        yield return _roomSpawner.Spawn(sourceRoom).AsCoroutine();
        Spawning = false;
    }
}
