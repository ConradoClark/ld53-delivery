using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

[RequireComponent(typeof(WeaponHit))]
public class FlipByXDirection : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer  SpriteRenderer { get; private set; }
    private WeaponHit _hit;

    protected override void OnAwake()
    {
        base.OnAwake();
        _hit = GetComponent<WeaponHit>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SpriteRenderer.flipX = _hit.Direction.x < 0;
    }
}
