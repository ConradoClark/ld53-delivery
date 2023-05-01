using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;

public class UI_WeaponSlot : BaseGameObject
{
    [field:SerializeField]
    public Weapon AssociatedWeapon { get; private set; }
    [field: SerializeField]
    public SpriteRenderer Icon { get; private set; }
    [field: SerializeField]
    public TMP_Text TextComponent { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        LoadWeaponData();
    }

    public void SetWeapon(Weapon weapon)
    {
        AssociatedWeapon = weapon;
    }

    private void LoadWeaponData()
    {
        if (AssociatedWeapon == null)
        {
            Icon.sprite = null;
            TextComponent.text = "";
            return;
        }
        Icon.sprite = AssociatedWeapon.WeaponIcon;
        TextComponent.text = AssociatedWeapon.WeaponName.ToUpper();
    }
}
