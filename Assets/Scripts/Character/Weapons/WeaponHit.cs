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

    public bool Critical { get; set; }

    public float Range { get; set; }

    public int BaseDamage { get; set; }
    [field:SerializeField]
    public string DamageType { get; set; }

    public event Action<LichtPhysicsObject> OnWeaponHitContact;

    [field: SerializeField]
    public TintFlash Flash { get; private set; }

    [field: SerializeField]
    public bool Stays { get; private set; }

    private List<LichtPhysicsObject> _activatedFor;

    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
    }

    protected override void OnEnable()
    {
        _activatedFor = new List<LichtPhysicsObject>();
        base.OnEnable();
        if (Critical && Flash != null)
        {
            Flash.Flash();
        }
    }

    public bool RegisterImpact(LichtPhysicsObject target)
    {
        var registered = false;
        if (!_activatedFor.Contains(target))
        {
            registered = true;
            OnWeaponHitContact?.Invoke(target);
            _activatedFor.Add(target);
        }

        if (Stays) return registered;
        EndEffect();
        return registered;
    }
}
