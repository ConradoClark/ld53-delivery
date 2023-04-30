using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class EnemyContactDetector : BaseGameRunner
{
    [field: SerializeField]
    public LayerMask LayerMask { get; private set; }
    [field: SerializeField]
    public Damageable Damageable { get; private set; }

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
            var hits = triggers.SelectMany(t => t.Item2).ToArray();
            foreach (var hitCollision in hits)
            {
                if (!LayerMask.Contains(hitCollision.Collider.gameObject.layer) ||
                    !_physics.TryGetPhysicsObjectByCollider(hitCollision.Collider, out var hitObject) ||
                    !hitObject.TryGetCustomObject<EnemyContactDamage>(out var hit) ||
                    hit.PhysicsObject.CompareTag(Trigger.PhysicsObject.tag)
                   )
                {
                    continue;
                }
                Damageable.Hit(new Damageable.DamageArgs
                {
                    Source = hit.PhysicsObject,
                    BaseDamage = hit.CalculateDamage(),
                    DamageType = "Physical"
                });

                yield return TimeYields.WaitMilliseconds(GameTimer, 150);
            }
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
