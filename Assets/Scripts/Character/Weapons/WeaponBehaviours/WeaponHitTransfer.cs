using System;
using System.Linq;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class WeaponHitTransfer : BaseGameObject
{
    [field:SerializeField]
    public WeaponHit Source { get; private set; }
    [field: SerializeField]
    public ScriptPrefab Target { get; private set; }
    [field: SerializeField]
    public float MainHitProportion { get; private set; }
    
    [field:SerializeField]
    public WeaponTargetFinder CustomFinder { get;private set; }

    private WeaponHitPoolManager _poolManager;
    private WeaponHitPool _pool;
    private int _originalBaseDamage;

    protected override void OnEnable()
    {
        base.OnEnable();

        _poolManager = _poolManager.FromScene();
        _pool = _poolManager.GetEffect(Target);

        _originalBaseDamage = Source.BaseDamage;
        Source.BaseDamage = Mathf.RoundToInt(Source.BaseDamage * MainHitProportion);

        Source.OnWeaponHitContact += Source_OnWeaponHitContact;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Source.OnWeaponHitContact -= Source_OnWeaponHitContact;
    }

    private void Source_OnWeaponHitContact(Licht.Unity.Physics.LichtPhysicsObject obj)
    {
        var target = CustomFinder == null ? Source.Target :
            CustomFinder.FindTargets(Source.Range, 1, out var targets) 
                ? targets.First() : Source.Target;

        _pool.TryGetFromPool(out _, hit =>
        {
            hit.Critical = Source.Critical;
            hit.BaseDamage = _originalBaseDamage;
            hit.ObjectStats = Source.ObjectStats;
            hit.Target = target;
            hit.Direction = Source.Direction;
            hit.transform.position = Source.transform.position;
        });
    }
}
