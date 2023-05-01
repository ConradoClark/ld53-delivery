using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;


[RequireComponent(typeof(CanBePickedUp))]
[RequireComponent(typeof(TreasureChest))]
public class AddWeaponOnPickup : BaseGameObject
{
    private CanBePickedUp _pickupInterface;
    private WeaponsManager _weaponsManager;
    private TreasureChest _treasureChest;

    protected override void OnAwake()
    {
        base.OnAwake();
        _pickupInterface = GetComponent<CanBePickedUp>();
        _treasureChest = GetComponent<TreasureChest>();
        _weaponsManager = _weaponsManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _pickupInterface.OnPickup += OnPickup;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _pickupInterface.OnPickup -= OnPickup;
    }

    private void OnPickup()
    {
        _weaponsManager.Equip(_treasureChest.Drop);
    }
}

