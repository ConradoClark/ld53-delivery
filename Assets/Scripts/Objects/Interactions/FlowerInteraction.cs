using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Objects.Stats;
using UnityEngine;

public class FlowerInteraction : InteractiveAction
{
    [field:SerializeField]
    public DialogueInteraction YouNeedPPToPerformThisAction { get; private set; }

    [field: SerializeField]
    public DialogueInteraction SuccessfullyPollinated { get; private set; }

    [field: SerializeField]
    public DialogueInteraction FullyPollinated { get; private set; }

    [field:SerializeField]
    public ScriptPrefab PollinateEffect { get; private set; }
    [field: SerializeField]
    public SpriteRenderer FlowerSpriteRenderer { get; private set; }
    [field: SerializeField]
    public Sprite SuccessSprite { get; private set; }
    [field: SerializeField]
    public int RequiredAmount { get; private set; }

    [field: SerializeField]
    public Vector3 EffectOffset { get; private set; }

    [field:SerializeField]
    public TintFlash FlowerTintEffect { get; private set; }

    private int _providedAmount;

    private PlayerIdentifier _player;
    private ObjectStats _playerStats;
    private UI_ScreenFlash _flash;
    private Color _startingColor;

    protected override void OnAwake()
    {
        base.OnAwake();
        _player = _player.FromScene();
        _flash = _flash.FromScene(true);
        _startingColor = FlowerSpriteRenderer.color;

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerStats = _player.PlayerStats.GetStats();
    }

    public override IEnumerable<IEnumerable<Action>> PerformAction()
    {
        if (_providedAmount >= RequiredAmount)
        {
            yield return FullyPollinated.PerformAction().AsCoroutine();
            yield break;
        }

        var pp = _playerStats.Ints[Constants.StatNames.PP];

        if (pp <= 0)
        {
            yield return YouNeedPPToPerformThisAction.PerformAction().AsCoroutine();
            yield break;
        }

        PollinateEffect.TrySpawnEffect(transform.position + EffectOffset, out _);
        FlowerTintEffect.Flash();

        _providedAmount += 1;
        _playerStats.Ints[Constants.StatNames.PP] -= 1;

        FlowerSpriteRenderer.color = Color.Lerp(_startingColor, Color.white,
            _providedAmount / (float)RequiredAmount);

        if (_providedAmount < RequiredAmount)
        {
            yield break;
        }

        yield return _flash.FadeIn().AsCoroutine();
        FlowerSpriteRenderer.sprite = SuccessSprite;
        yield return _flash.FadeOut().AsCoroutine();

        yield return SuccessfullyPollinated.PerformAction().AsCoroutine();
    }
}
