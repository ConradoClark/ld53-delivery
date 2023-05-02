using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using Licht.Unity.Physics;
using Licht.Unity.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerInteract : BaseGameRunner
{
    [field: SerializeField]
    public MultiCollisionTrigger Trigger { get; private set; }

    [field: SerializeField]
    public InputActionReference ConfirmAction { get; private set; }

    [field: SerializeField]
    public GameObject InteractIcon { get; private set; }

    private LichtPhysics _physics;
    private bool _showingIcon;

    private Camera _gameCamera;
    private Camera _uiCamera;

    private InteractiveObject _activeInteractive;

    [field:SerializeField]
    public AudioSource InteractSound { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        _physics = this.GetLichtPhysics();
        _gameCamera = Camera.main;
        _uiCamera = SceneObject<UICamera>.Instance().Camera;
    }

    protected override IEnumerable<IEnumerable<Action>> Handle()
    {
        if (Trigger.GetTriggers(out var triggers))
        {
            var hits = triggers.FirstOrDefault().Item2;
            foreach (var hitCollision in hits)
            {
                if (!_physics.TryGetPhysicsObjectByCollider(hitCollision.Collider, out var hitObject) ||
                    !hitObject.TryGetCustomObject<InteractiveObject>(out var interactive)
                   )
                {
                    continue;
                }

                if (!_showingIcon || _activeInteractive!=interactive)
                {
                    _activeInteractive = interactive;
                    _showingIcon = true;
                    InteractIcon.SetActive(true);
                    var pos = interactive.transform.position + interactive.Offset;
                    InteractIcon.transform.position = _uiCamera.ViewportToWorldPoint(
                        _gameCamera.WorldToViewportPoint(pos));
                }

                if (ConfirmAction.action.WasPerformedThisFrame())
                {
                    if (InteractSound != null)
                    {
                        InteractSound.pitch = 1f + Random.value * 0.2f;
                        InteractSound.PlayOneShot(InteractSound.clip);
                    }
                    _showingIcon = false;
                    InteractIcon.SetActive(false);
                    yield return interactive.Action.PerformAction().AsCoroutine();
                }
            }
        }
        else
        {
            if (_showingIcon)
            {
                _showingIcon = false;
                InteractIcon.SetActive(false);
            }
        }

        yield return TimeYields.WaitOneFrameX;
    }
}
