using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.CharacterControllers;
using Licht.Unity.Objects;
using UnityEngine;

public class BasicCharacterAnimator : BaseGameObject
{
    [field:SerializeField]
    public SpriteRenderer MainSpriteRenderer { get; private set; }

    [field: SerializeField]
    public LichtTopDownMoveController MoveController { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        if (MoveController.LatestDirection.x == 0) return;

        MainSpriteRenderer.flipX = MoveController.LatestDirection.x < 0;
    }
}
