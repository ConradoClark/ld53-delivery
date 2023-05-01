using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

public class WeaponsManager : BaseGameObject
{
    [field:SerializeField]
    public GameObject WeaponsContainer { get; private set; }
    [field: SerializeField]
    public UI_WeaponSlot Slot1 { get; private set; }
    [field: SerializeField]
    public UI_WeaponSlot Slot2 { get; private set; }
    [field: SerializeField]
    public UI_WeaponSlot Slot3 { get; private set; }

    private UI_WeaponSlot[] _slots;

    public bool OnGlobalCooldown { get; private set; }
    private PlayerIdentifier _player;

    protected override void OnAwake()
    {
        base.OnAwake();
        _slots = new[] { Slot1, Slot2, Slot3 };
        _player = _player.FromScene();
    }

    public void Equip(Weapon weapon)
    {
        if (_slots.All(s => s.AssociatedWeapon != null))
        {
            RemoveWeakestWeapon();
        }

        foreach (var slot in _slots)
        {
            if (slot.AssociatedWeapon != null) continue;
            weapon.transform.SetParent(WeaponsContainer.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.Source = _player.PlayerStats;
            slot.SetWeapon(weapon);
            break;
        }
    }

    private void RemoveWeakestWeapon()
    {
        var weakest = _slots.Select(t => (t, t.AssociatedWeapon.CalculateRank()))
            .OrderBy(t => t.Item2)
            .First();

        weakest.t.AssociatedWeapon.gameObject.SetActive(false);
        weakest.t.SetWeapon(null);
    }

    public IEnumerable<IEnumerable<Action>> AddGlobalCooldown(float timeInMs)
    {
        OnGlobalCooldown = true;
        yield return TimeYields.WaitMilliseconds(GameTimer, timeInMs);
        OnGlobalCooldown = false;
    }
}
