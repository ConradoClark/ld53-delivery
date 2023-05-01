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
    [field: SerializeField]
    public RoomObject RoomObject { get; set; }
    [field: SerializeField]
    public ObjectStats BaseStats { get; private set; }

    [field:SerializeField]
    public EnemyType Type { get; private set; }

    [field: SerializeField]
    public Vector3 HPBarPositionOffset { get; private set; }

    [field: SerializeField]
    public bool FixedObject { get; private set; }

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

    private UI_FloatingHPCounter _currentCounter;
    protected override void OnAwake()
    {
        base.OnAwake();
        _counterPool = SceneObject<FloatingHPCounterPoolManager>.Instance(true).GetEffect(FloatingHPCounterPrefab);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_currentCounter != null)
        {
            _currentCounter.EndEffect();
        }

        _counterPool.TryGetFromPool(out _currentCounter, counter =>
        {
            counter.Source = this;
            counter.PositionOffset = HPBarPositionOffset;
            counter.Stats = CurrentStats;
        });

        if (!FixedObject) return;
        Randomize();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _currentCounter = null;
    }

    public event Action OnRandomize;

    public void Randomize()
    {
        if (CurrentStats!=null) DestroyImmediate(CurrentStats);

        CurrentStats = Instantiate(BaseStats);
        CurrentStats.Ints[Constants.StatNames.HP] = CurrentStats.Ints[Constants.StatNames.MaxHP];

        if (_currentCounter != null)
        {
            _currentCounter.EndEffect();
        }

        _counterPool.TryGetFromPool(out _currentCounter, counter =>
        {
            counter.Source = this;
            counter.PositionOffset = HPBarPositionOffset;
            counter.Stats = CurrentStats;
        });

        OnRandomize?.Invoke();

        if (Type is EnemyType.Normal or EnemyType.Boss) return;
    }

    private void Update()
    {
        if (_currentCounter == null || CurrentStats == null) return;
        _currentCounter.Source = this;
        _currentCounter.Stats = CurrentStats;
    }
}