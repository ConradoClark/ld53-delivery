using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Events;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class FlowerManager : BaseGameObject
{
    public int TotalFlowerCount { get; private set; }
    public ScriptIntegerStat CurrentFlowers { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        CurrentFlowers = new ScriptIntegerStat();

        var allFlowers = FindObjectsByType<FlowerInteraction>(FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        TotalFlowerCount = allFlowers.Length;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ObserveEvent(FlowerInteraction.FlowerEvents.OnPollinate, OnPollinate);
    }

    private void OnPollinate()
    {
        CurrentFlowers.Stat += 1;
    }
}
