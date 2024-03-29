﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class PlayerStatsHolder : StatsHolder
{
    [field:SerializeField]
    public ObjectStats Stats { get; private set; }

    private ObjectStats _clonedStats;

    public override ObjectStats GetStats()
    {
        if (_clonedStats == null)
        {
            _clonedStats = Instantiate(Stats);
        }
        return _clonedStats;
    }
}
