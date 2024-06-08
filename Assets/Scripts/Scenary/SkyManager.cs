using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    [Header("<color=blue>SkyBox</color>")]
    [SerializeField] private float _timeModifier = .05f;

    private void LateUpdate()
    {
        RenderSettings.skybox.SetFloat($"_Rotation", Time.time * _timeModifier);
    }
}
