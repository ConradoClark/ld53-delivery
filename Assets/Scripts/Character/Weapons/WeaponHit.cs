using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;

public class WeaponHit : PooledComponent
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }
    public ObjectStats ObjectStats { get; set; }
    public Transform Target { get; set; }
    public Vector3 Direction { get; set; }
    public int BaseDamage { get; set; }
    [field:SerializeField]
    public string DamageType { get; set; }

    public event Action<LichtPhysicsObject> OnWeaponHitContact;

    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
    }

    public void RegisterImpact(LichtPhysicsObject target)
    {
        OnWeaponHitContact?.Invoke(target);
        EndEffect();
    }
}
