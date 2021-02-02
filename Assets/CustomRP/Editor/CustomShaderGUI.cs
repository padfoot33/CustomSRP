using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class CustomShaderGUI : ShaderGUI
 {
    #region PUBLIC_VARS
    #endregion

    #region PRIVATE_VARS
    private MaterialEditor editor;
    private Object[] materials;
    private MaterialProperty[] properties;
    private bool Clipping { set => SetProperty("_Clipping", "_CLIPPING", value); }
    bool HasPremultiplyAlpha => HasProperty("_PremulAlpha");
    private bool PremultiplyAlpha { set => SetProperty("_PremulAlpha", "_PREMULTIPLY_ALPHA", value); }
    private BlendMode SrcBlend { set => SetProperty("_SrcBlend", (float)value); }
    private BlendMode DstBlend { set => SetProperty("_DstBlend", (float)value); }
    private bool ZWrite { set => SetProperty("_ZWrite", value ? 1f : 0f); }

    private bool showPresets;
    private RenderQueue renderQueue
    {
        set
        {
            foreach (Material m in materials)
                m.renderQueue = (int)value;
        }
    }

	enum ShadowMode
	{
		On, Clip, Dither, Off
	}

	ShadowMode Shadows
	{
		set
		{
			if (SetProperty("_Shadows", (float)value))
			{
				SetKeyword("_SHADOWS_CLIP", value == ShadowMode.Clip);
				SetKeyword("_SHADOWS_DITHER", value == ShadowMode.Dither);
			}
		}
	}
	#endregion

	#region UNITY_CALLBACKS
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
		EditorGUI.BeginChangeCheck();
		base.OnGUI(materialEditor, properties);
        editor = materialEditor;
        materials = materialEditor.targets;
        this.properties = properties;

        EditorGUILayout.Space();
        showPresets = EditorGUILayout.Foldout(showPresets, "Presets", true);
        if (showPresets)
        {
            OpaquePreset();
            ClipPreset();
            FadePreset();
            TransparentPreset();
        }
		if (EditorGUI.EndChangeCheck())
		{
			SetShadowCasterPass();
		}
	}
	#endregion

	#region PUBLIC_FUNCTIONS
	#endregion

	#region PRIVATE_FUNCTIONS
	private void SetShadowCasterPass()
	{
		MaterialProperty shadows = FindProperty("_Shadows", properties, false);
		if (shadows == null || shadows.hasMixedValue)
		{
			return;
		}
		bool enabled = shadows.floatValue < (float)ShadowMode.Off;
		foreach (Material m in materials)
		{
			m.SetShaderPassEnabled("ShadowCaster", enabled);
		}
	}
	private void OpaquePreset()
	{
		if (PresetButton("Opaque"))
		{
			Clipping = false;
			PremultiplyAlpha = false;
			SrcBlend = BlendMode.One;
			DstBlend = BlendMode.Zero;
			ZWrite = true;
			renderQueue = RenderQueue.Geometry;
		}
	}

	private void ClipPreset()
	{
		if (PresetButton("Clip"))
		{
			Clipping = true;
			PremultiplyAlpha = false;
			SrcBlend = BlendMode.One;
			DstBlend = BlendMode.Zero;
			ZWrite = true;
			renderQueue = RenderQueue.AlphaTest;
		}
	}

	private void FadePreset()
	{
		if (PresetButton("Fade"))
		{
			Clipping = false;
			PremultiplyAlpha = false;
			SrcBlend = BlendMode.SrcAlpha;
			DstBlend = BlendMode.OneMinusSrcAlpha;
			ZWrite = false;
			renderQueue = RenderQueue.Transparent;
		}
	}

	private void TransparentPreset()
	{
		if (HasPremultiplyAlpha && PresetButton("Transparent"))
		{
			Clipping = false;
			PremultiplyAlpha = true;
			SrcBlend = BlendMode.One;
			DstBlend = BlendMode.OneMinusSrcAlpha;
			ZWrite = false;
			renderQueue = RenderQueue.Transparent;
		}
	}

	private bool PresetButton(string name)
	{
		if (GUILayout.Button(name))
		{
			editor.RegisterPropertyChangeUndo(name);
			return true;
		}
		return false;
	}

	private bool HasProperty(string name) =>
		FindProperty(name, properties, false) != null;

	private void SetProperty(string name, string keyword, bool value)
	{
		if (SetProperty(name, value ? 1f : 0f))
		{
			SetKeyword(keyword, value);
		}
	}

	private bool SetProperty(string name, float value)
	{
		MaterialProperty property = FindProperty(name, properties, false);
		if (property != null)
		{
			property.floatValue = value;
			return true;
		}
		return false;
	}

	private void SetKeyword(string keyword, bool enabled)
	{
		if (enabled)
		{
			foreach (Material m in materials)
			{
				m.EnableKeyword(keyword);
			}
		}
		else
		{
			foreach (Material m in materials)
			{
				m.DisableKeyword(keyword);
			}
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