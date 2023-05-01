using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

[RequireComponent(typeof(WeaponHit))]
public class EffectOnWeaponHitContact : BaseGameObject
{
    [field:SerializeField]
    public ScriptPrefab Effect { get; private set; }

    [field: SerializeField]
    public ScriptPrefab CritEffect { get; private set; }

    private WeaponHit _hit;

    protected override void OnAwake()
    {
        base.OnAwake();
        _hit = GetComponent<WeaponHit>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _hit.OnWeaponHitContact += OnWeaponHitContact;
    }

    private void OnWeaponHitContact(Licht.Unity.Physics.LichtPhysicsObject obj)
    {
        if (!_hit.Critical)
        {
            Effect.TrySpawnEffect(transform.position, out _);
            return;
        }
        CritEffect.TrySpawnEffect(transform.position, out _);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _hit.OnWeaponHitContact -= OnWeaponHitContact;
    }
}
