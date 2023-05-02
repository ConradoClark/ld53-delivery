using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Generation;
using Licht.Interfaces.Generation;
using Licht.Unity.Extensions;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureChest : PooledComponent, IGenerator<int,float>
{
    [field: SerializeField]
    public WeaponDefinition[] WeaponBases { get; private set; }
    public float RarityFactor { get; set; }
    
    [field: SerializeField]
    public CanBePickedUp PickupInterface { get; private set; }

    public Weapon Drop { get; private set; }

    private WeaponsManager _weaponsManager;

    public bool Dropped { get; set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        _weaponsManager = _weaponsManager.FromScene(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ActiveOnInitialization)
        {
            Dropped = true;
            OnActivation();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Dropped = false;
    }

    [Serializable]
    public struct WeaponDefinition :IWeighted<float>
    {
        public string Name;
        public string Description;
        public GameObject WeaponBase;
        public float MinRarity;
        public int BaseDamage;
        public float Chance;
        public float Weight => Chance;
        public Color Color;
    }

    protected override void OnActivation()
    {
        if (!Dropped) return;

        base.OnActivation();

        var possibleWeapons = WeaponBases.Where(w => RarityFactor >= w.MinRarity)
            .ToArray();

        var dice = new WeightedDice<WeaponDefinition>(possibleWeapons, this);
        var chosenWeapon = dice.Generate();

        var weaponObject = Instantiate(chosenWeapon.WeaponBase);
        var weapon = weaponObject.GetComponent<Weapon>();

        weapon.BaseDamage = chosenWeapon.BaseDamage;
        weapon.WeaponName = chosenWeapon.Name;
        Drop = weapon;

        PickupInterface.Description = chosenWeapon.Description;
        PickupInterface.DescriptionColor = chosenWeapon.Color;
    }

    public int Seed { get; set; }
    public float Generate()
    {
        return Random.value;
    }
}
