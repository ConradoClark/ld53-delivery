using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using UnityEngine;

public class TintFlashOnDamage : BaseGameObject
{
    [field:SerializeField]
    public Damageable Damageable { get; private set; }

    [field: SerializeField]
    public TintFlash TintFlash { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        Damageable.OnDamage += Damageable_OnDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Damageable.OnDamage -= Damageable_OnDamage;
    }

    private void Damageable_OnDamage(Damageable.DamageArgs obj)
    {
        TintFlash.Flash();
    }
}
