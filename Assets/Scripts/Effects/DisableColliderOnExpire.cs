using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class DisableColliderOnExpire : BaseGameObject
{
    [field: SerializeField]
    public Collider2D Collider { get; set; }

    [field: SerializeField]
    public WeaponHit Disable { get; set; }

    [field: SerializeField]
    public PooledComponent PooledComponent { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        PooledComponent.OnEffectOver += PooledComponent_OnEffectOver;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PooledComponent.OnEffectOver -= PooledComponent_OnEffectOver;
    }

    private void PooledComponent_OnEffectOver()
    {
        Collider.enabled = false;
        Disable.enabled = false;
    }
}
