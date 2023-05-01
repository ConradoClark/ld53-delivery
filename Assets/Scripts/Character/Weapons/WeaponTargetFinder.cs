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


    public virtual bool FindTargets(float range, int amount, out Transform[] targets)
    {
        var objects = GameObject.FindGameObjectsWithTag(TargetTag);

        var closestInRange = objects
            .Select(obj => IsInRange(range, obj.transform))
            .Where(obj => obj.isInRange)
            .OrderBy(obj => obj.distance)
            .Take(amount)
            .Select(obj=>obj.target)
            .ToArray();

        if (closestInRange.Length == 0)
        {
            targets = Array.Empty<Transform>();
            return false;
        }

        targets = closestInRange;
        return true;
    }

    private (Transform target, bool isInRange, float distance) IsInRange(float range, Transform target)
    {
        var distance = Vector2.Distance(target.position, transform.position);
        return (target, distance <= range, distance);
    }
}
