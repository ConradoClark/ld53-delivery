using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;
using Random = System.Random;

internal class ActivateObjectOnDeath : BaseGameObject
{
    [field: SerializeField]
    public EnemyDieWhenZeroHP EnemyDeath { get; private set; }

    [field: SerializeField]
    public GameObject Target { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        EnemyDeath.OnDeath += EnemyDeath_OnDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EnemyDeath.OnDeath -= EnemyDeath_OnDeath;
    }

    private void EnemyDeath_OnDeath()
    {
        Target.SetActive(true);
    }
}
