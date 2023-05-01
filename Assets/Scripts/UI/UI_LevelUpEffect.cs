using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UI_LevelUpEffect : BaseGameRunner
{
    [field:SerializeField]
    public TMP_Text TextComponent { get;private set; }
    [field: SerializeField]
    public Color[] PossibleColors { get; private set; }

    private Vector3 _originalPosition;
    protected override void OnAwake()
    {
        base.OnAwake();
        _originalPosition = transform.position;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        transform.position = _originalPosition;
        DefaultMachinery.AddBasicMachine(FlashColors());

        yield return transform.GetAccessor()
            .Position
            .Y
            .Increase(0.5f)
            .Over(1f)
            .UsingTimer(GameTimer)
            .Build();
    }

    private IEnumerable<IEnumerable<Action>> FlashColors()
    {
        while (ComponentEnabled)
        {
            TextComponent.color = PossibleColors[Random.Range(0, PossibleColors.Length)];
            yield return TimeYields.WaitMilliseconds(GameTimer, 100);
        }
    }
}
