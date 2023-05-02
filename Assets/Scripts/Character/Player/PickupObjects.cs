using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObjects : BaseGameRunner
{
    [field: SerializeField]
    public InputActionReference  Confirm { get; private set; }

    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }

    [field: SerializeField]
    public MultiCollisionTrigger Trigger { get; private set; }

    public CanBePickedUp CurrentHover { get;private set; }

    [field: SerializeField]
    public AudioSource PickupSound { get; private set; }

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
                    !pickupObject.TryGetCustomObject<CanBePickedUp>(out var pickup))
                {
                    CurrentHover = null;
                    continue;
                }

                CurrentHover = pickup;

                if (!pickup.PickedUpOnTouch && !Confirm.action.WasPerformedThisFrame())
                {
                    break;
                }

                if (PickupSound != null)
                {
                    PickupSound.PlayOneShot(pickup.Sound == null ? PickupSound.clip : pickup.Sound);
                }
                pickup.Pickup();
                break;
            }
        }
        else
        {
            CurrentHover = null;
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
