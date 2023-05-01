using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class SpawnEnemyPeriodically : BaseGameRunner
{
    private EnemyPoolManager _enemyPoolManager;

    [field: SerializeField]
    public RoomObject RoomObject { get; private set; }

    [field:SerializeField]
    public ScriptPrefab EnemyPrefab { get; private set; }

    [field: SerializeField]
    public float DelayInSeconds { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        _enemyPoolManager = _enemyPoolManager.FromScene(true);
    }
    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        yield return TimeYields.WaitSeconds(GameTimer, DelayInSeconds);
        _enemyPoolManager.GetEffect(EnemyPrefab).TryGetFromPool(out _, e =>
        {
            e.transform.position = transform.position;
            e.RoomObject.SetRoomInstance(RoomObject.Room);
            e.Randomize();
        });
    }


}
