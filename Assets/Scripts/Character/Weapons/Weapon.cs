
using System;
using System.Collections.Generic;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

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
        while (ComponentEnabled)
        {
            if (Source != null && TargetFinder.FindTarget(out var target))
            {
                TrySpawnWeaponHit(out _, target);
                yield return TimeYields.WaitMilliseconds(GameTimer, CooldownInMs);
                continue;
            }

            yield return TimeYields.WaitOneFrameX;
        }
    }

    protected bool TrySpawnWeaponHit(out WeaponHit weaponHit, Transform target)
    {
        return WeaponHitPoolManager.GetEffect(WeaponHitPrefab)
            .TryGetFromPool(out weaponHit, hit =>
            {
                hit.BaseDamage = CalculateDamage();
                hit.ObjectStats = Source.GetStats();
                hit.Target = target;
                var offsetDir = (Vector3)((Vector2)(target.position - transform.position)).normalized;
                hit.Direction = offsetDir;
                hit.transform.position = transform.position + offsetDir * 0.5f;
            });
    }

    private int CalculateDamage()
    {
        return BaseDamage;
    }
}
