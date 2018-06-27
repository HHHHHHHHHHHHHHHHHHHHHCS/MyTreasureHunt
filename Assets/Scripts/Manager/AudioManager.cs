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

    private bool isMute = false;

    public void OnInit()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchMuteState(int? _isMute = null)
    {
        isMute = _isMute.HasValue ? (_isMute == 0 ? false : true) : !isMute;
        if (isMute)
        {
            StopBGM();
        }
        else
        {
            PlayBGM();
        }
        DataManager.Instance.UpdateData(PlayerAttribute.IsMute, isMute ? 1 : 0);
        DataManager.Instance.SaveData();
    }

    public void PlayClip(AudioClip clip)
    {
        if (!isMute)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM()
    {
        if (!isMute)
        {
            audioSource.Play();
        }
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
