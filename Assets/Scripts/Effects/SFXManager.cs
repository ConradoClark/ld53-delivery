﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;
using UnityEngine;

public class SFXManager : BaseGameObject
{
    [field:SerializeField]
    public AudioSource GenericAudioSource { get; private set; }
}
