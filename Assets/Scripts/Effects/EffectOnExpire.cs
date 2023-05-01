using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class EffectOnExpire : BaseGameObject
{
    [field:SerializeField]
    public Vector3 Offset { get; private set; }

    [field: SerializeField]
    public ScriptPrefab ScriptPrefab { get; private set; }

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
        ScriptPrefab.TrySpawnEffect(transform.position + Offset, out _);
    }
}
