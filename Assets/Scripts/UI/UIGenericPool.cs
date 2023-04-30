using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Pooling;
using UnityEngine;

public class UIGenericPool : GenericPrefabPool<PooledComponent>
{
    private void OnEnable()
    {
        this.transform.localScale = new Vector3(32, 32, 32);
    }
}
