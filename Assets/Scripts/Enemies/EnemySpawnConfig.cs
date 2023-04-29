using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct EnemySpawnConfig
{
    public int EnemyMinLevel;
    public int EnemyMaxLevel;
    public EnemyPack[] PossiblePacks;
}
