using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Dialogue : BaseGameObject
{
    [field: SerializeField]
    public TMP_Text TextComponent { get; private set; }

    [field: SerializeField]
    public InputActionReference SkipInput { get; private set; }

    [field: SerializeField]
    public float CharacterFrequencyInMs { get; private set; }

    [field:SerializeField]
    public GameObject Cursor { get; private set; }

    public IEnumerable<IEnumerable<Action>> ShowText(string text, bool showCursor=true)
    {
        TextComponent.text = "";
        var pressedSkip = false;
        Cursor.SetActive(false);

        yield return TimeYields.WaitOneFrameX;

        for (var i = 0; i < text.Length; i++)
        {
            TextComponent.text = text[..i];
            yield return TimeYields.WaitMilliseconds(GameTimer, CharacterFrequencyInMs, 
                breakCondition:() =>
                {
                    if (SkipInput.action.WasPerformedThisFrame())
                    {
                        pressedSkip = true;
                    }
                    return pressedSkip;
                });

            if (!pressedSkip && !SkipInput.action.WasPerformedThisFrame()) continue;

            TextComponent.text = text;
            break;
        }

        TextComponent.text = text;

        Cursor.SetActive(true);
        yield return TimeYields.WaitOneFrameX;

        while (!SkipInput.action.WasPerformedThisFrame())
        {
            yield return TimeYields.WaitOneFrameX;
        }
    }

    public void Clear()
    {
        TextComponent.text = "";
        Cursor.SetActive(false);
    }
}
