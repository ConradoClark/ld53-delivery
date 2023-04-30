using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class CanBePickedUp : BaseGameObject
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public bool PickedUpOnTouch { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
    }

    public event Action OnPickup;

    public void Pickup()
    {
        OnPickup?.Invoke();
    }
}
