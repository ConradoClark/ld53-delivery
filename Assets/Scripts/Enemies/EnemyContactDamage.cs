using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using UnityEngine;

public class EnemyContactDamage : BaseGameObject
{
    [field:SerializeField]
    public Enemy Enemy { get; private set; }

    [field: SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }

    [field: SerializeField]
    public int BaseDamage { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        PhysicsObject.AddCustomObject(this);
    }

    public int CalculateDamage()
    {
        return Enemy.CurrentStats.Ints[Constants.StatNames.Attack] + BaseDamage;
    }
}
