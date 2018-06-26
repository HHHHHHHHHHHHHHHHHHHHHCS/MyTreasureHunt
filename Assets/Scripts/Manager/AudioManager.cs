using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip button;
    public AudioClip dig;
    public AudioClip end;
    public AudioClip hoe;
    public AudioClip hurt;
    public AudioClip die;
    public AudioClip move;
    public AudioClip door;
    public AudioClip pass;
    public AudioClip enemy;
    public AudioClip tnt;
    public AudioClip map;
    public AudioClip pick;
    public AudioClip flag;
    public AudioClip why;
    public AudioClip winbg;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
}
