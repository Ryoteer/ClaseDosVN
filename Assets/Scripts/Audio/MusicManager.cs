using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=purple>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    private void Start()
    {
        if (!_source)
        {
            _source = GetComponent<AudioSource>();
        }
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 25);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 25);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0) value = 0.0001f;

        _mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 25);
    }

    public void PlayClip(AudioClip clip)
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = clip;

        _source.Play();
    }
}
