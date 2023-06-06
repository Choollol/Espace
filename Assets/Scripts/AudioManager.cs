using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float bgmVolume;
    public float sfxVolume;

    public AudioSource boxNormalDestroy;
    public AudioSource blasterFire;
    public AudioSource generatorActivated;
    public AudioSource click;
    public AudioSource jump;
    public AudioSource canister;
    public AudioSource boxHeavyDestroy;
    public AudioSource stepSound;

    public AudioSource theme;

    public Camera mainCamera;

    public float volumeInterval;
    public GameObject bgmSoundBar;
    public GameObject sfxSoundBar;

    private List<AudioSource> bgmList;
    private List<AudioSource> sfxList;
    
    void Start()
    {
        sfxList = new List<AudioSource>()
        {
            boxNormalDestroy, blasterFire, generatorActivated, click, jump, canister, boxHeavyDestroy, stepSound,
        };
        bgmList = new List<AudioSource>()
        {
            theme
        };
        UpdateBGMVolume(0);
        UpdateSFXVolume(0);
    }

    void Update()
    {
        transform.position = mainCamera.transform.position;
    }
    public void IncreaseBGMVolume()
    {
        UpdateBGMVolume(bgmVolume + volumeInterval);
    }
    public void DecreaseBGMVolume()
    {
        UpdateBGMVolume(bgmVolume - volumeInterval);
    }
    public void IncreaseSFXVolume()
    {
        UpdateSFXVolume(sfxVolume + volumeInterval);
    }
    public void DecreaseSFXVolume()
    {
        UpdateSFXVolume(sfxVolume - volumeInterval);
    }
    public void UpdateBGMVolume(float volume)
    {
        if (volume > 1.05f)
        {
            return;
        }
        bgmVolume = volume;
        foreach (AudioSource bgm in bgmList)
        {
            bgm.volume = bgmVolume;
        }
        UpdateSoundBars();
    }
    public void UpdateSFXVolume(float volume)
    {
        if (volume > 1.05f)
        {
            return;
        }
        sfxVolume = volume;
        foreach (AudioSource sfx in sfxList)
        {
            sfx.volume = sfxVolume;
        }
        UpdateSoundBars();
    }
    public void UpdateSoundBars()
    {
        for (int i = 0; i < 10; i++)
        {
            bgmSoundBar.transform.GetChild(i).gameObject.SetActive(false);
            sfxSoundBar.transform.GetChild(i).gameObject.SetActive(false);
            if (i < bgmList.Count)
            {
                bgmList[i].volume = (float)decimal.Round((decimal)bgmList[0].volume, 1, System.MidpointRounding.ToEven);
            }
            if (i < sfxList.Count)
            {
                sfxList[i].volume = (float)decimal.Round((decimal)sfxList[0].volume, 1, System.MidpointRounding.ToEven);
            }
        }
        for (int i = 0; i < Mathf.Round(bgmList[0].volume * 10); i++)
        {
            bgmSoundBar.transform.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < Mathf.Round(sfxList[0].volume * 10); i++)
        {
            sfxSoundBar.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void PlayClickSound()
    {
        click.Play();
    }
}
