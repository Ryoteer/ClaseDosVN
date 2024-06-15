using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [Header("<color=orange>Values</color>")]
    [Range(0f, 1f)][SerializeField] private float _initMasterVol = 1f;
    [Range(0f, 1f)][SerializeField] private float _initMusicVol = .25f;
    [Range(0f, 1f)][SerializeField] private float _initSFXVol = 1f;

    [Header("<color=orange>UI</color>")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        _masterSlider.value = _initMasterVol;
        SetMasterVolume(_initMasterVol);
        _musicSlider.value = _initMusicVol;
        SetMusicVolume(_initMusicVol);
        _sfxSlider.value = _initSFXVol;
        SetSFXVolume(_initSFXVol);
    }

    public void SetMasterVolume(float value)
    {
        MusicManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        MusicManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        MusicManager.Instance.SetSFXVolume(value);
    }
}
