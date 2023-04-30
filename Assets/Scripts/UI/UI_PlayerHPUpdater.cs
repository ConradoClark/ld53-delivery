using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_PlayerHPUpdater : BaseGameObject
{
    [field:SerializeField]
    public TMP_Text TextComponent { get; private set; }
    private PlayerIdentifier _player;

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

        stats.Ints.GetStat(Constants.StatNames.HP).OnChange += UI_PlayerHPUpdater_OnChange;
        TextComponent.text = stats.Ints[Constants.StatNames.HP].ToString();
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
        TextComponent.text = obj.NewValue.ToString();
    }
}
