using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : ControllerBaseModel
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] private CinemachineShake shaker;
    public CinemachineVirtualCamera ActiveCamera;

    public override void Initialize()
    {
        base.Initialize();
        shaker.Initialize();
    }

    public void ChangeCamera(int index)
    {
        ActiveCamera.SetActiveGameObject(false);
        ActiveCamera = virtualCameras[index];
        ActiveCamera.SetActiveGameObject(true);
    }

    public void Shake(float intensity, float time) 
    {
        shaker.Shake(intensity, time);
    }
}