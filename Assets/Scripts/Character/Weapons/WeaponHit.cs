﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Pooling;
using UnityEngine;

public class WeaponHit : PooledComponent
{
    public ObjectStats ObjectStats { get; set; }
    public Transform Target { get; set; }

    public Vector3 Direction { get; set; }
}
