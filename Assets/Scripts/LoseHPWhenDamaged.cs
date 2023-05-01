using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class LoseHPWhenDamaged : BaseGameObject
{
    [field: SerializeField]
    public StatsHolder StatsHolder { get; private set; }
    private Damageable _damageable;

    protected override void OnAwake()
    {
        base.OnAwake();
        _damageable = GetComponent<Damageable>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _damageable.OnDamage += OnDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _damageable.OnDamage -= OnDamage;
    }

    private void OnDamage(Damageable.DamageArgs obj)
    {
        var stats = StatsHolder.GetStats();
        var currentHP = stats.Ints[Constants.StatNames.HP];
        stats.Ints[Constants.StatNames.HP] = Math.Clamp(currentHP - CalculateDamage(obj, stats), 0, currentHP);
    }

    private int CalculateDamage(Damageable.DamageArgs obj, ObjectStats stats)
    {
        return Math.Max(1, obj.BaseDamage - stats.Ints[Constants.StatNames.Defense]);
    }
}