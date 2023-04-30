using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class PickupObjects : BaseGameRunner
{
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }

    [field: SerializeField]
    public MultiCollisionTrigger Trigger { get; private set; }

    private LichtPhysics _physics;
    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (Trigger.GetTriggers(out var triggers))
        {
            var hits = triggers.FirstOrDefault().Item2 ?? Array.Empty<CollisionResult>();
            foreach (var hitCollision in hits)
            {
                if (!LayerMask.Contains(hitCollision.Collider.gameObject.layer) ||
                    !_physics.TryGetPhysicsObjectByCollider(hitCollision.Collider, out var pickupObject) ||
                    !pickupObject.TryGetCustomObject<CanBePickedUp>(out var pickup) ||
                    !pickup.PickedUpOnTouch
                   )
                {
                    continue;
                }

                pickup.Pickup();
            }
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
