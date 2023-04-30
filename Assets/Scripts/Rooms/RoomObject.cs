using Licht.Unity.Objects;
using Unity.VisualScripting;
using UnityEngine;

public class RoomObject : BaseGameObject
{
    public Room Room { get; private set; }

    [field:SerializeField]
    public bool AlwaysActive { get; private set; }

    private bool _addedToRoom;

    protected override void OnAwake()
    {
        base.OnAwake();
        Room = GetComponentInParent<Room>(true);
        if (Room == null) return;

        _addedToRoom = true;
        Room.Add(this);
        Room.OnDeactivation += Room_OnDeactivation;
        Room.OnActivation += Room_OnActivation;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Room == null || _addedToRoom) return;

        _addedToRoom = true;
        Room.Add(this);
        Room.OnDeactivation += Room_OnDeactivation;
        Room.OnActivation += Room_OnActivation;
    }

    private void OnDestroy()
    {
        if (Room == null) return;
        Room.OnDeactivation -= Room_OnDeactivation;
        Room.OnActivation -= Room_OnActivation;
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

    public void SetRoomInstance(Room room)
    {
        Room = room;
    }
}
