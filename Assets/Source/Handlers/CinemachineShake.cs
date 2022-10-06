using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : ObjectModel
{
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] float shakeTimer;
    CinemachineBasicMultiChannelPerlin channel;
    float _intensity = 0;

    public override void Initialize()
    {
        base.Initialize();
        channel = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        channel.m_AmplitudeGain = 0f;
        _intensity = 0;
    }

    public void Shake(float intensity, float time)
    {

        _intensity = intensity;
        channel.m_AmplitudeGain = _intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                channel.m_AmplitudeGain = 0f;
            }
        }
    }
}