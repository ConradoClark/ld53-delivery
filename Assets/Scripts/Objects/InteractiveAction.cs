using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;

public abstract class InteractiveAction : BaseGameObject
{
    public abstract IEnumerable<IEnumerable<Action>> PerformAction();
}
