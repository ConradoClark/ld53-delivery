using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Licht.Unity.Objects;
using UnityEngine;

public class WeaponTargetFinder : BaseGameObject
{
    [field: SerializeField]
    [field: TagField]
    public string TargetTag { get; protected set; }
    public bool FindTarget(out Transform target)
    {
        var maybe = GameObject.FindGameObjectsWithTag(TargetTag).FirstOrDefault();

        if (maybe == null)
        {
            target = null;
            return false;
        }

        target = maybe.transform;
        return true;
    }
}
