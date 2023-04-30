using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using UnityEngine;

internal class DialogueInteraction : InteractiveAction
{
    [field:SerializeField]
    [field:Multiline]
    public string[] Texts { get; private set; }
    private UI_DialogueBar _dialogueBar;

    protected override void OnAwake()
    {
        base.OnAwake();
        _dialogueBar = _dialogueBar.FromScene(true);
    }

    public override IEnumerable<IEnumerable<Action>> PerformAction()
    {
        yield return _dialogueBar.Show(Texts).AsCoroutine();
    }
}
