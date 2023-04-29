using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Interfaces.Generation;
using Licht.Unity.Objects;

[Serializable]
public struct EnemyPack : IWeighted<float>
{
    public ScriptPrefab[] Prefabs;
    public float Chance;

    public float Weight => Chance;
}
