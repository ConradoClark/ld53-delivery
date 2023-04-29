using System.Linq;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.Physics.CollisionDetection;
using UnityEngine;

public class MultiCollisionTrigger : BaseGameObject
{
    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public Basic2DCollisionDetector[] Colliders { get; private set; }

    private LichtPhysics _physics;
    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
    }

    public bool GetTriggers(out (Basic2DCollisionDetector, CollisionResult[])[] results)
    {
        results =
            Colliders.Select(c => (c, c.Triggers.Where(t => t.TriggeredHit).ToArray())).ToArray();

        return results.All(c => c.Item2.Any());
    }

    public bool GetTriggers<T>(out (Basic2DCollisionDetector, T[])[] results) where T : class
    {
        results =
            Colliders.Select(c => (c,
                c.Triggers.Where(t => t.TriggeredHit)
                    .Select(t => GetObjectFromCollision<T>(t.Collider))
                    .Where(t => t != null)
                    .ToArray()

                )).ToArray();

        return results.All(c => c.Item2.Any());
    }

    private T GetObjectFromCollision<T>(Collider2D objCollider) where T : class
    {
        if (_physics.TryGetPhysicsObjectByCollider(objCollider, out var obj)
            && obj.TryGetCustomObject(out T comp))
        {
            return comp;
        }

        return null;
    }

}
