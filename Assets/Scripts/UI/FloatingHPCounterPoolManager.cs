using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Pooling;
using UnityEngine;

public class FloatingHPCounterPoolManager : CustomPrefabManager<FloatingHPCounterPool, UI_FloatingHPCounter>
{
    private void OnEnable()
    {
        if (Effects.Count == 0) return;

        Effects.First().Value.transform.localScale = new Vector3(32, 32, 32);
    }
}
