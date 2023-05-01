using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using Licht.Unity.UI;
using TMPro;
using UnityEngine;

public class UI_ShowPickupDescription : BaseGameObject
{
    private Camera _gameCamera;
    private Camera _uiCamera;

    [field:SerializeField]
    public PickupObjects Pickup { get; private set; }
    [field: SerializeField]
    public SpriteRenderer Box { get; private set; }
    [field: SerializeField]
    public TMP_Text TextComponent { get; private set; }

    [field: SerializeField]
    public SpriteRenderer Button { get; private set; }

    [field: SerializeField]
    public Vector3 Offset { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        _gameCamera = SceneObject<GameCamera>.Instance().Camera;
        _uiCamera = SceneObject<UICamera>.Instance().Camera;
    }

    private void LateUpdate()
    {
        if (Pickup.CurrentHover == null || !Pickup.CurrentHover.ShowDescription)
        {
            Box.enabled = TextComponent.enabled = Button.enabled = false;
            return;
        }

        transform.position = _uiCamera.ViewportToWorldPoint(
            _gameCamera.WorldToViewportPoint(Pickup.CurrentHover.transform.position + Offset));

        Box.enabled = TextComponent.enabled = Button.enabled = true;
        TextComponent.color = Pickup.CurrentHover.DescriptionColor;
        TextComponent.text = Pickup.CurrentHover.Description;
    }
}
