using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

[RequireComponent(typeof(WeaponHit))]
[RequireComponent(typeof(LichtPhysicsObject))]
public class StraightLineBehaviour : BaseGameRunner
{
    [field:SerializeField]
    public float Speed { get; private set; }
    private WeaponHit _weaponHit;
    private LichtPhysicsObject _physicsObject;

    protected override void OnAwake()
    {
        base.OnAwake();
        _weaponHit = GetComponent<WeaponHit>();
        _physicsObject = GetComponent<LichtPhysicsObject>();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        while (!_weaponHit.IsEffectOver)
        {
            _physicsObject.ApplySpeed(_weaponHit.Direction * Speed);
            yield return TimeYields.WaitOneFrameX;
        }
    }
}
