using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class CustomRenderPipeline : RenderPipeline
{
    #region PUBLIC_VARS
    #endregion

    #region PRIVATE_VARS
    private CameraRenderer cameraRenderer = new CameraRenderer();
    private bool useDynamicBatching, useGPUInstancing;
    private ShadowSettings shadowSettings;
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_FUNCTIONS
    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher,
        ShadowSettings shadowSettings)
    {
        this.shadowSettings = shadowSettings;
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    #endregion

    #region PROTECTED_FUNCTIONS
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
            cameraRenderer.Render(context, camera, useDynamicBatching, useGPUInstancing, shadowSettings);

    }
    #endregion

    #region CO-ROUTINES
    #endregion

    #region EVENT_HANDLERS
    #endregion

    #region UI_CALLBACKS
    #endregion






}
