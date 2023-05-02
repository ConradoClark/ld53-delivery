using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects.Stats;
using Licht.Unity.Physics;
using Licht.Unity.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponHit : PooledComponent
{
    [field:SerializeField]
    public LichtPhysicsObject PhysicsObject { get; private set; }
    public ObjectStats ObjectStats { get; set; }
    public Transform Target { get; set; }
    public Vector3 Direction { get; set; }

    public bool Critical { get; set; }

    public float Range { get; set; }

    public int BaseDamage { get; set; }
    [field:SerializeField]
    public string DamageType { get; set; }

    public event Action<LichtPhysicsObject> OnWeaponHitContact;

    [field: SerializeField]
    public TintFlash Flash { get; private set; }

    [field: SerializeField]
    public bool Stays { get; private set; }

    private List<LichtPhysicsObject> _activatedFor;

    [field:SerializeField]
    public AudioSource ImpactSound { get; private set; }


    private SFXManager _sfxManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        PhysicsObject.AddCustomObject(this);
        _sfxManager = _sfxManager.FromScene();
    }

    protected override void OnEnable()
    {
        _activatedFor = new List<LichtPhysicsObject>();
        base.OnEnable();
        if (Critical && Flash != null)
        {
            Flash.Flash();
        }
    }

    public bool RegisterImpact(LichtPhysicsObject target)
    {
        var registered = false;
        if (!_activatedFor.Contains(target))
        {
            registered = true;
            OnWeaponHitContact?.Invoke(target);
            _activatedFor.Add(target);

            if (ImpactSound != null)
            {
                _sfxManager.GenericAudioSource.pitch = 0.8f + Random.value * 0.4f;
                _sfxManager.GenericAudioSource.PlayOneShot(ImpactSound.clip);
            }
        }

        if (Stays) return registered;
        EndEffect();
        return registered;
    }
}
