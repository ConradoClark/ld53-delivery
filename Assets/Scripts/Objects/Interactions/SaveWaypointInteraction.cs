using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class SaveWaypointInteraction :InteractiveAction
{
    [field:SerializeField]
    public DialogueInteraction TextInteraction { get; private set; }

    [field: SerializeField]
    public Waypoint Waypoint { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public override IEnumerable<IEnumerable<Action>> PerformAction()
    {
        yield return TextInteraction.PerformAction().AsCoroutine();
        Waypoint.Save();
    }
}
