using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;

public class EnemyDropItemOnDeath : BaseGameObject
{
    [field: SerializeField]
    public ScriptPrefab TreasurePrefab { get; private set; }

    [field: SerializeField]
    public EnemyDieWhenZeroHP EnemyDeath { get; private set; }

    private TreasureChestPoolManager _poolManager;
    private TreasureChestPool _pool;
    [field: SerializeField]
    public StatsHolder EnemyStats { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        _poolManager = _poolManager.FromScene(true);
        if (_poolManager == null) return;
        _pool = _poolManager.GetEffect(TreasurePrefab);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _poolManager ??= _poolManager.FromScene(true);
        _pool ??= _poolManager.GetEffect(TreasurePrefab);
        EnemyDeath.OnDeath += EnemyDeath_OnDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyDeath.OnDeath -= EnemyDeath_OnDeath;
    }

    private void EnemyDeath_OnDeath()
    {
        var stats = EnemyStats.GetStats();
        if (Random.value > stats.Floats[Constants.StatNames.DropChance]) return;

        _pool.TryGetFromPool(out _, comp =>
        {
            comp.Component.transform.position = transform.position;
            comp.RarityFactor = stats.Floats[Constants.StatNames.Rarity];
        });
    }
}
