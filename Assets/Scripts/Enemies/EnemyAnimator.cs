using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class EnemyAnimator : BaseGameObject
{

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public SpriteRenderer MainSprite { get; private set; }

    private void Update()
    {
        MainSprite.flipX = PhysicsObject.LatestNonZeroSpeed.x < 0;
    }
}
