using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Pooling;
using UnityEngine;

public class Enemy : PooledComponent
{
    public RoomObject RoomObject { get; set; }
    [field: SerializeField]
    public ObjectStats BaseStats { get; private set; }

    [field:SerializeField]
    public EnemyType Type { get; private set; }

    [Serializable]
    public enum EnemyType
    {
        Normal,
        Magic,
        Rare,
        Boss
    }
    
    public ObjectStats CurrentStats { get; private set; }

    public void Randomize()
    {
        if (CurrentStats!=null) DestroyImmediate(CurrentStats);
        CurrentStats = Instantiate(BaseStats);

        if (Type is EnemyType.Normal or EnemyType.Boss) return;
    }
}