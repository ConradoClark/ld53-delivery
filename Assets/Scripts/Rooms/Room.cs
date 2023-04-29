using System;
using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;

public class Room : BaseGameObject
{
    [field:SerializeField]
    public bool IsActive { get; private set; }
    public List<RoomObject> RoomObjects { get; private set; }
    
    public event Action OnActivation;
    public event Action OnDeactivation;

    protected override void OnAwake()
    {
        base.OnAwake();
        RoomObjects ??= new List<RoomObject>();
        if (!IsActive)
        {
            Deactivate();
        }
    }

    public void Add(RoomObject obj)
    {
        RoomObjects ??= new List<RoomObject>();
        if (RoomObjects.Contains(obj)) return;
        RoomObjects.Add(obj);
    }

    public void Deactivate()
    {
        IsActive = false;
        OnDeactivation?.Invoke();
    }

    public void Activate()
    {
        IsActive = true;
        OnActivation?.Invoke();
    }


}
