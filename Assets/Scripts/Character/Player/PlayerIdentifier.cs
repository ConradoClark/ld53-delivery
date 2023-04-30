using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class PlayerIdentifier : BaseGameObject
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public StatsHolder PlayerStats { get; private set; }

    public Room CurrentRoom { get; set; }
}
