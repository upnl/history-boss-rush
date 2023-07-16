using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip[] sfxClips;

    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void PlayBGM()
    {
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySfx(int index)
    {
        sfxPlayer.PlayOneShot(sfxClips[index]);
    }
}
