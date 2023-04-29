using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;

public abstract class StatsHolder : BaseGameObject
{
    public abstract ObjectStats GetStats();
}
