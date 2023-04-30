using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using UnityEngine;

public class AICondition_NoticePlayer : BaseAICondition
{
    [field:SerializeField]
    public float MinDistance { get; private set; }
    public bool Noticed { get; private set; }
    private PlayerIdentifier _player;
    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Noticed = false;
    }

    public override bool CheckCondition()
    {
        if (Noticed) return false;

        Noticed = Vector2.Distance(_player.transform.position, transform.position) < MinDistance;
        return Noticed;
    }
}
