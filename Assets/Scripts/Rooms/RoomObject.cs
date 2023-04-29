using Licht.Unity.Objects;
using UnityEngine;

public class RoomObject : BaseGameObject
{
    public Room Room { get; private set; }

    [field:SerializeField]
    public bool AlwaysActive { get; private set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        Room = GetComponentInParent<Room>(true);
        if (Room == null) return;

        Room.Add(this);
        Room.OnDeactivation += Room_OnDeactivation;
        Room.OnActivation += Room_OnActivation;
    }

    private void Room_OnActivation()
    {
        gameObject.SetActive(true);
    }

    private void Room_OnDeactivation()
    {
        if (AlwaysActive) return;
        gameObject.SetActive(false);
    }
}
