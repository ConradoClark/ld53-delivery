using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

[RequireComponent(typeof(CanBePickedUp))]
[RequireComponent(typeof(PooledComponent))]
public class ExpireOnPickup : BaseGameObject
{
    private PooledComponent _pooledComponent;
    private CanBePickedUp _pickupInterface;

    protected override void OnAwake()
    {
        base.OnAwake();
        _pickupInterface = GetComponent<CanBePickedUp>();
        _pooledComponent = GetComponent<PooledComponent>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _pickupInterface.OnPickup += OnPickup;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _pickupInterface.OnPickup -= OnPickup;
    }

    private void OnPickup()
    {
        _pooledComponent.EndEffect();
    }
}
