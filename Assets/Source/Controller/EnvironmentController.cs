using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : ControllerBaseModel
{
    [SerializeField] private VisualEnvironmentModel[] visualEnvironments;
    [SerializeField] private Material bgMaterial;

    [EditorButton]
    public void LoadVisualModel(int visualIndex)
    {
        VisualEnvironmentModel visualEnvironment = visualEnvironments[visualIndex];
        bgMaterial.SetColor("_TopColor", visualEnvironment.BackgroundColor.TopColor);
        bgMaterial.SetColor("_BottomColor", visualEnvironment.BackgroundColor.BottomColor);
        bgMaterial.SetFloat("_Center", visualEnvironment.BackgroundColor.Center);
        bgMaterial.SetFloat("_Spread", visualEnvironment.BackgroundColor.Spread);

        RenderSettings.fogColor = visualEnvironment.FogColor;
    }
}