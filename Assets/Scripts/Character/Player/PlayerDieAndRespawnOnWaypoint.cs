using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class PlayerDieAndRespawnOnWaypoint : BaseGameObject
{
    private PlayerIdentifier _player;
    private WaypointManager _waypointManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _waypointManager = _waypointManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        var stats = _player.PlayerStats.GetStats();
        if (stats == null) return;

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange += UI_PlayerHPUpdater_OnChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        var stats = _player.PlayerStats.GetStats();
        if (stats == null) return;

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange -= UI_PlayerHPUpdater_OnChange;
    }

    private void UI_PlayerHPUpdater_OnChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        if (obj.NewValue > 0) return;

        DefaultMachinery.AddUniqueMachine("player_die", 
            UniqueMachine.UniqueMachineBehaviour.Cancel, Die());
    }

    private IEnumerable<IEnumerable<Action>> Die()
    {
        var stats = _player.PlayerStats.GetStats();
        stats.Ints[Constants.StatNames.HP] = stats.Ints[Constants.StatNames.MaxHP];

        _player.transform.position = _waypointManager.CurrentWaypoint.transform.position;
        if (_player.CurrentRoom != null)_player.CurrentRoom.Deactivate();
        _waypointManager.CurrentWaypoint.RoomObject.Room.Activate();

        yield break;
    }
}
