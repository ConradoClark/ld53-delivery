using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_PlayerStatUpdater : BaseGameObject
{
    [field:SerializeField]
    public TMP_Text TextComponent { get; private set; }
    private PlayerIdentifier _player;

    [field: SerializeField]
    public string Stat { get;private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        var stats = _player.PlayerStats.GetStats();
        if (stats == null) return;

        stats.Ints.GetStat(Stat).OnChange += UI_Player_OnChange;
        TextComponent.text = stats.Ints[Stat].ToString();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        var stats = _player.PlayerStats.GetStats();
        if (stats == null) return;

        stats.Ints.GetStat(Stat).OnChange -= UI_Player_OnChange;
    }

    private void UI_Player_OnChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        TextComponent.text = obj.NewValue.ToString();
    }
}
