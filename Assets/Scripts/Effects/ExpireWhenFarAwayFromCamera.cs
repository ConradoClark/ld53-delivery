using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class ExpireWhenFarAwayFromCamera : BaseGameObject
{
    [field:SerializeField]
    public PooledComponent PooledComponent { get; private set; }

    [field: SerializeField]
    public float Distance { get; private set; }

    private Camera _mainCamera;

    protected override void OnAwake()
    {
        base.OnAwake();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Vector2.Distance(PooledComponent.transform.position,
                _mainCamera.transform.position) <= Distance) return;

        PooledComponent.EndEffect();
    }
}
