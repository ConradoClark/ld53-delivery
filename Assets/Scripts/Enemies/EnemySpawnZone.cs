using System;
using System.Collections.Generic;
using System.Linq;
using Licht.Impl.Generation;
using Licht.Impl.Orchestration;
using Licht.Interfaces.Generation;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RoomObject))]
public class EnemySpawnZone : BaseGameRunner, IGenerator<int, float>
{
    public float MinSpawnFrequencyInSeconds { get; private set; }
    public float MaxSpawnFrequencyInSeconds { get; private set; }
    public float LastSpawnTime { get; private set; }
    public float NextSpawnTime { get; private set; }

    [field: SerializeField]
    public EnemySpawnConfig EnemySpawnConfig { get; private set; }

    [field: SerializeField]
    public BoxCollider2D ZoneBounds { get; private set; }

    [field: SerializeField]
    public bool SpawnOncePerActivation { get; private set; }

    [field: SerializeField]
    public bool SpawnOnlyOffscreen { get; private set; }

    private RoomObject _roomObject;
    private EnemyPoolManager _enemyPoolManager;
    private List<Enemy> _enemies;
    private PlayerIdentifier _player;
    private bool _spawned;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _enemies = new List<Enemy>();
        _roomObject = GetComponent<RoomObject>();
        _enemyPoolManager = _enemyPoolManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _spawned = false;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        var shouldSpawn = (!SpawnOncePerActivation || !_spawned)
            && (!SpawnOnlyOffscreen ||
                (SpawnOnlyOffscreen && Vector2.Distance(_player.transform.position, transform.position) > 15));

        if (GameTimer.TotalElapsedTimeInMilliseconds > NextSpawnTime && shouldSpawn)
        {
            NextSpawnTime = GameTimer.TotalElapsedTimeInMilliseconds
                            + Random.Range(MinSpawnFrequencyInSeconds, MaxSpawnFrequencyInSeconds) * 1000;

            if (_enemies.Count == 0 || _enemies.All(e => e.IsEffectOver))
            {
                LastSpawnTime = GameTimer.TotalElapsedTimeInMilliseconds;
                Spawn();
            }
        }

        yield return TimeYields.WaitOneFrameX;
    }

    private void Spawn()
    {
        _spawned = true;
        var packs = new WeightedDice<EnemyPack>(EnemySpawnConfig.PossiblePacks,
             this);

        var selectedPack = packs.Generate();
        _enemies.Clear();

        foreach (var enemyPrefab in selectedPack.Prefabs)
        {
            var pool = _enemyPoolManager.GetEffect(enemyPrefab);
            if (!pool.TryGetFromPool(out var enemy, e =>
                {
                    var spawnPos = GetRandomPositionInZone();
                    e.transform.position = spawnPos;
                    e.RoomObject = _roomObject;
                    e.Randomize();
                })) continue;

            _enemies.Add(enemy);
        }
    }

    private Vector3 GetRandomPositionInZone()
    {
        var x = Random.Range(ZoneBounds.bounds.min.x, ZoneBounds.bounds.max.x);
        var y = Random.Range(ZoneBounds.bounds.min.y, ZoneBounds.bounds.max.y);

        return new Vector3(x, y);
    }

    public int Seed { get; set; }
    public float Generate()
    {
        return Random.value;
    }
}
