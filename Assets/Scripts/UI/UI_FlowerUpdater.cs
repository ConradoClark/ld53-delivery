using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_FlowerUpdater : BaseGameObject
{
    [field: SerializeField]
    public TMP_Text TextComponent { get; private set; }

    private FlowerManager _flowerManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _flowerManager = _flowerManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _flowerManager.CurrentFlowers.OnChange += CurrentFlowers_OnChange;
    }

    private void CurrentFlowers_OnChange(Licht.Unity.Objects.Stats.ScriptStat<int>.StatUpdate obj)
    {
        var percent = obj.NewValue / (float)_flowerManager.TotalFlowerCount;
        var result = Mathf.RoundToInt(percent * 100);

        TextComponent.text = $"{result}%";
    }
}
