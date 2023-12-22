using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsEffects : MonoBehaviour
{
    [Header("AudioSource")]
    public AudioSource playerSoundsEffectAudioSource;

    [Header("Sounds List")]
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip shootSound;
    public AudioClip stunSound;
    public AudioClip eatSound;
    public AudioClip dashSound;
    public AudioClip pickupSound;
}
