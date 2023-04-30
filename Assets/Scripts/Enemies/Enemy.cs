﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Pooling;
using UnityEngine;

public class Enemy : PooledComponent
{
    [field: SerializeField]
    public ScriptPrefab FloatingHPCounterPrefab { get; private set; }
    public RoomObject RoomObject { get; set; }
    [field: SerializeField]
    public ObjectStats BaseStats { get; private set; }

    [field:SerializeField]
    public EnemyType Type { get; private set; }

    [field: SerializeField]
    public Vector3 HPBarPositionOffset { get; private set; }

    [Serializable]
    public enum EnemyType
    {
        Normal,
        Magic,
        Rare,
        Boss
    }
    
    public ObjectStats CurrentStats { get; private set; }

    private FloatingHPCounterPool _counterPool;
    protected override void OnAwake()
    {
        base.OnAwake();
        _counterPool = SceneObject<FloatingHPCounterPoolManager>.Instance(true).GetEffect(FloatingHPCounterPrefab);
    }

    protected override void OnActivation()
    {
        base.OnActivation();
        _counterPool.TryGetFromPool(out _, counter =>
        {
            counter.Source = this;
            counter.PositionOffset = HPBarPositionOffset;
            counter.Stats = CurrentStats;
        });
    }

    public void Randomize()
    {
        if (CurrentStats!=null) DestroyImmediate(CurrentStats);
        CurrentStats = Instantiate(BaseStats);

        if (Type is EnemyType.Normal or EnemyType.Boss) return;
    }
}