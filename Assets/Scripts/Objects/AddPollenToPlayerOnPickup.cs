using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

[RequireComponent(typeof(CanBePickedUp))]
public class AddPollenToPlayerOnPickup : BaseGameObject
{
    [field:SerializeField]
    public int Amount { get; private set; }
    private CanBePickedUp _pickupInterface;
    private PlayerIdentifier _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _pickupInterface = GetComponent<CanBePickedUp>();
        _player = _player.FromScene();
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
        var stats= _player.PlayerStats.GetStats();
        var currentPP = stats.Ints[Constants.StatNames.PP];

        stats.Ints[Constants.StatNames.PP] = Math.Clamp(currentPP + Amount,
            currentPP, stats.Ints[Constants.StatNames.MaxPP]);
    }
}
