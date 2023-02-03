using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private Material skyMaterial;
    [SerializeField] private Material lightsMaterial;
    private static readonly int AtmosphereThickness = Shader.PropertyToID("_AtmosphereThickness");

    
    [SerializeField] private float dayDuration = 20;
    [SerializeField] private float sunsetDuration = 5;
    [SerializeField] private float nightDuration = 20;
    [SerializeField] private float sunriseDuration = 5;
    private static readonly int EmissionColor = Shader.PropertyToID("Color_F736FCDA");


    private void Start()
    {
        skyMaterial.SetFloat(AtmosphereThickness, 0.04f);
        DOTween.Sequence()
            .Append(DOTween.To(() => skyMaterial.GetFloat(AtmosphereThickness),
                f => skyMaterial.SetFloat(AtmosphereThickness, f),
                0.5f, sunriseDuration))
            .Join(DOTween.To(()=>lightsMaterial.GetColor(EmissionColor),
                c => lightsMaterial.SetColor(EmissionColor, c), Color.clear, sunriseDuration))
            .AppendInterval(dayDuration)
            .Append(DOTween.To(() => skyMaterial.GetFloat(AtmosphereThickness),
                f => skyMaterial.SetFloat(AtmosphereThickness, f),
                0.04f, sunsetDuration))
            .Join(DOTween.To(()=>lightsMaterial.GetColor(EmissionColor),
                c => lightsMaterial.SetColor(EmissionColor, c), Color.yellow, sunsetDuration))
            .AppendInterval(nightDuration)
            .SetLoops(-1)
            .Play();
    }
}
