using UnityEngine;

public class MeshBall : MonoBehaviour
{
    #region PUBLIC_VARS

    public Mesh mesh = default;

    public Material material = default;

    public float sphereRadius;
    public Vector2 randomeScale;
    #endregion

    #region PRIVATE_VARS
    private static int 
        baseColorId = Shader.PropertyToID("_BaseColor"),
        metallicId = Shader.PropertyToID("_Metallic"),
        smoothnessId = Shader.PropertyToID("_Smoothness");

    private float[]
        metallic = new float[1023],
        smoothness = new float[1023];

    private static int arraySize = 1023;
    private Matrix4x4[] matrices = new Matrix4x4[arraySize];
    private Vector4[] baseColors = new Vector4[arraySize];

    private MaterialPropertyBlock block;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        SetColorAndMatrices();
    }

    private void Update()
    {
        SetMaterialPropertyBlock();
        Graphics.DrawMeshInstanced(mesh, 0, material, matrices, arraySize, block);
    }
    #endregion

    #region PUBLIC_FUNCTIONS
    #endregion

    #region PRIVATE_FUNCTIONS
    private void SetColorAndMatrices()
    {
        for (int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = Matrix4x4.TRS
            (
                Random.insideUnitSphere * sphereRadius,
                Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                Vector3.one * Random.Range(randomeScale.x, randomeScale.y)
            );
            baseColors[i] = new Vector4(Random.value, Random.value, Random.value,
                Random.Range(randomeScale.x, randomeScale.y));
            metallic[i] = Random.value < 0.25f ? 1f : 0f;
            smoothness[i] = Random.Range(0.05f, 0.95f);
        }
    }
    private void SetMaterialPropertyBlock()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColorId, baseColors);
            block.SetFloatArray(metallicId, metallic);
            block.SetFloatArray(smoothnessId, smoothness);
        }
    }
    #endregion

    #region CO-ROUTINES
    #endregion

    #region EVENT_HANDLERS
    #endregion

    #region UI_CALLBACKS
    #endregion

    
    
}