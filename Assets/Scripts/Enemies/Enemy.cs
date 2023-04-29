using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Pooling;

public class Enemy : PooledComponent
{
    public RoomObject RoomObject { get; set; }

    public void Randomize()
    {

    }
}