using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class Damageable : BaseGameObject
{

    [field:SerializeField]
    public bool IsInvulnerable { get; set; }

    [Serializable]
    public struct DamageArgs
    {
        public int BaseDamage;
        public string DamageType;
        public LichtPhysicsObject Source;
    }

    public event Action<DamageArgs> OnDamage;

    public void Hit(DamageArgs args)
    {
        if (IsInvulnerable) return;
        OnDamage?.Invoke(args);
    }
}
