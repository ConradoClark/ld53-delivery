using Licht.Unity.Effects;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDropPollenOnDeath : BaseGameObject
{
    [field:SerializeField]
    public ScriptPrefab PollenPrefab { get; private set; }

    [field: SerializeField]
    public EnemyDieWhenZeroHP EnemyDeath { get; private set; }

    [field: SerializeField]
    public int MinQuantity { get; private set; }

    [field: SerializeField]
    public int MaxQuantity { get; private set; }

    private EffectsManager _effectsManager;
    private PrefabPool _pool;

    protected override void OnAwake()
    {
        base.OnAwake();
        _effectsManager = _effectsManager.FromScene(true);
        _pool = _effectsManager.GetEffect(PollenPrefab);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyDeath.OnDeath += EnemyDeath_OnDeath;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        EnemyDeath.OnDeath -= EnemyDeath_OnDeath;
    }

    private void EnemyDeath_OnDeath()
    {
        var amount = Random.Range(MinQuantity, MaxQuantity + 1);
        _pool.TryGetManyFromPool(amount, out _, (comp, index) =>
        {
            comp.Component.transform.position = transform.position;
        });
    }
}
