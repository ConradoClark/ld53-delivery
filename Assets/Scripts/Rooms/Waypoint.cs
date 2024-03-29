﻿using System;
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

public class Waypoint : BaseGameObject
{
    private WaypointManager _waypointManager;
    [field: SerializeField]
    public RoomObject RoomObject { get; private set; }

    private PlayerIdentifier _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        RoomObject = GetComponent<RoomObject>();
        _waypointManager = _waypointManager.FromScene();
        _player = _player.FromScene();
    }

    public void Save()
    {
        _waypointManager.CurrentWaypoint = this;
        var stats = _player.PlayerStats.GetStats();
        stats.Ints[Constants.StatNames.HP] = stats.Ints[Constants.StatNames.MaxHP];
    }
}
