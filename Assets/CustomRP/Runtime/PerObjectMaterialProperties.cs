using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{

    #region PUBLIC_VARS
    [SerializeField] Color baseColor = Color.white;
    [SerializeField, Range(0f, 1f)] 
        float cutoff = 0.5f, metallic = 0f, smoothness = 0.5f;
    #endregion

    #region PRIVATE_VARS
    private static int
        baseColorId = Shader.PropertyToID("_BaseColor"),
        cutoffId = Shader.PropertyToID("_Cutoff"),
        metallicId = Shader.PropertyToID("_Metallic"),
        smoothnessId = Shader.PropertyToID("_Smoothness");

    private static MaterialPropertyBlock block;
    #endregion

    #region UNITY_CALLBACKS
    void Awake()
    {
        OnValidate();
    }
    private void OnValidate()
    {
        if (block == null)
            block = new MaterialPropertyBlock();

        block.SetColor(baseColorId, baseColor);
        block.SetFloat(cutoffId, cutoff);
        block.SetFloat(metallicId, metallic);
        block.SetFloat(smoothnessId, smoothness);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    #endregion

    #region PRIVATE_FUNCTIONS
    #endregion

    #region CO-ROUTINES
    #endregion

    #region EVENT_HANDLERS
    #endregion

    #region UI_CALLBACKS
    #endregion




}