using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using UnityEngine;

[RequireComponent(typeof(RoomObject))]
public class RoomSameSceneSpawner : RoomSpawner
{
    private RoomObject _roomObject;
    private PlayerIdentifier _player;
    private UI_ScreenTransition _screenTransition;

    protected override void OnAwake()
    {
        base.OnAwake();
        
        _roomObject = GetComponent<RoomObject>();
        _player = _player.FromScene();
        _screenTransition = _screenTransition.FromScene();
    }

    public override IEnumerable<IEnumerable<Action>> Spawn(Room sourceRoom)
    {
        _player.MoveController.BlockMovement(this);
        yield return _screenTransition.FadeIn().AsCoroutine();

        sourceRoom.Deactivate();
        _roomObject.Room.Activate();

        _player.transform.position = transform.position;

        _player.MoveController.UnblockMovement(this);
        yield return _screenTransition.FadeOut().AsCoroutine();
    }
}
