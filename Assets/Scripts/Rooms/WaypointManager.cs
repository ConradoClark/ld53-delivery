using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class WaypointManager : BaseGameObject
{
    [field:SerializeField]
    public Waypoint CurrentWaypoint { get; set; }
}
