using System;
using System.Collections.Generic;
using Licht.Unity.Extensions;
using UnityEngine;

[RequireComponent(typeof(RoomObject))]
public class RoomSameSceneSpawner : RoomSpawner
{
    private RoomObject _roomObject;
    private PlayerIdentifier _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _roomObject = GetComponent<RoomObject>();
        _player = _player.FromScene();
    }

    public override IEnumerable<IEnumerable<Action>> Spawn(Room sourceRoom)
    {
        sourceRoom.Deactivate();
        _roomObject.Room.Activate();

        _player.transform.position = transform.position;
        yield break;
    }
}
