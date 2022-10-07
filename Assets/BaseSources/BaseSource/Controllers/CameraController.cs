using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : ControllerBaseModel
{
    public static CameraController Instance;
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] private CinemachineShake shaker;
    public CinemachineVirtualCamera ActiveCamera;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
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

    [EditorButton]
    public void OnEnterBridge()
    {
        ChangeCamera(1);
        PlayerController.Instance.ExtraForwardSpeed = 5f;
        Invoke(nameof(onExitBridge), 3.25f);
    }

    private void onExitBridge() 
    {
        ChangeCamera(0);
        PlayerController.Instance.ExtraForwardSpeed = 0f;
    }
}