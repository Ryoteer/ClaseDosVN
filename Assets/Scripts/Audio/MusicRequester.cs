using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("<color=purple>Audio</color>")]
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
        MusicManager.Instance.PlayClip(_clip);
    }
}
