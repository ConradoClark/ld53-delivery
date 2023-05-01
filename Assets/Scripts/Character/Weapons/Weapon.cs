
using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : BaseGameObject
{
    [field: SerializeField]
    public int BaseDamage { get; private set; }
    [field: SerializeField]
    public StatsHolder Source { get; set; }

    [field: SerializeField]
    public float CooldownInMs { get; protected set; }

    [field: SerializeField]
    public WeaponTargetFinder TargetFinder { get; protected set; }

    [field: SerializeField]
    public ScriptPrefab WeaponHitPrefab { get; protected set; }

    protected WeaponHitPoolManager WeaponHitPoolManager;

    [field: SerializeField]
    public float Range { get; private set; }

    [field:SerializeField]
    public Sprite WeaponIcon { get; private set; }

    [field: SerializeField]
    public string WeaponName { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        WeaponHitPoolManager = WeaponHitPoolManager.FromScene(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DefaultMachinery.AddBasicMachine(Handle());
    }

    protected IEnumerable<IEnumerable<Action>> Handle()
    {
        var stats = Source.GetStats();
        while (ComponentEnabled)
        {
            if (Source != null && TargetFinder.FindTargets(CalculateRange(), stats.Ints[Constants.StatNames.AoE],
                    out var targets))
            {
                foreach (var target in targets)
                {
                    TrySpawnWeaponHit(out _, target);
                }
                
                yield return TimeYields.WaitMilliseconds(GameTimer, CalculateCoolDown());
                continue;
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    protected bool TrySpawnWeaponHit(out WeaponHit weaponHit, Transform target)
    {
        var crit = CalculateCrit();
        return WeaponHitPoolManager.GetEffect(WeaponHitPrefab)
            .TryGetFromPool(out weaponHit, hit =>
            {
                hit.Critical = crit;
                hit.BaseDamage = CalculateDamage(crit);
                hit.ObjectStats = Source.GetStats();
                hit.Target = target;
                var offsetDir = (Vector3)((Vector2)(target.position - transform.position)).normalized;
                hit.Direction = offsetDir;
                hit.transform.position = transform.position + offsetDir * 0.5f;
            });
    }

    private bool CalculateCrit()
    {
        var stats = Source.GetStats();

        var baseCrit = 0.02f;
        var luckCrit = 0.04f * stats.Ints[Constants.StatNames.Luck];

        return Random.value < baseCrit + luckCrit;
    }

    private int CalculateDamage(bool critical)
    {
        var stats = Source.GetStats();
        var damage = BaseDamage + stats.Ints[Constants.StatNames.Attack];
        return critical ? Mathf.RoundToInt(damage * 1.5f) : damage;
    }

    private float CalculateRange()
    {
        return Range;
    }

    private float CalculateCoolDown()
    {
        var stats = Source.GetStats();
        return Mathf.Clamp(CooldownInMs - (stats.Ints[Constants.StatNames.Speed] - 1) * 50, 50, CooldownInMs);
    }
}
