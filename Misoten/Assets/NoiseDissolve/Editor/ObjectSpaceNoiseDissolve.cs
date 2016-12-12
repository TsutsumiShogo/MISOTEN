using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectSpaceNoiseDissolve : MaterialEditor
{
	Material material {
		get {
			return target as Material;
		}
	}

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();

		if (isVisible == false) {
			return;
		}

		serializedObject.Update ();
		DrawPropertys ();
		serializedObject.ApplyModifiedProperties ();
	}

	void DrawPropertys ()
	{
		EditorGUI.BeginChangeCheck ();

		// Color
		Color color = EditorGUILayout.ColorField ("Color", material.GetColor ("_Color"));

		// Texture
		Texture mainTex = EditorGUILayout.ObjectField ("AlbedoTex", material.GetTexture ("_MainTex"), typeof(Texture), false) as Texture;
		Texture metalTex = EditorGUILayout.ObjectField ("MetallicTex", material.GetTexture ("_MetallicTex"), typeof(Texture), false) as Texture;

		// Metal
		float metallic = 0;
		if (metalTex == null) {
			metallic = EditorGUILayout.Slider ("Metallic", material.GetFloat ("_Metallic"), 0, 1);
		}
		float smoothness = EditorGUILayout.Slider ("Smoothness", material.GetFloat ("_Glossiness"), 0, 1);

		EditorGUILayout.Space ();

		// Noise
		float noiseSize = EditorGUILayout.FloatField ("NoiseSize", material.GetFloat ("_NoiseSize"));

		// Dissolve
		float dissolvePercentage = EditorGUILayout.Slider ("DissolvePercentage", material.GetFloat ("_DissolvePercentage"), 0, 1);

		// Rim
		float rimSize = EditorGUILayout.Slider ("RimSize", material.GetFloat ("_RimSize") * 10, 0, 1);
		Color rimColor = EditorGUILayout.ColorField ("RimColor", material.GetColor ("_RimColor"));
		float rimIntensity = EditorGUILayout.FloatField ("RimIntensity", material.GetFloat ("_RimIntensity"));
		float enhancedRimColor = EditorGUILayout.Toggle ("EnhancedRimColor", material.GetFloat ("_EnhancedRimColor") == 1 ? true : false) == true ? 1 : 0;

		if (EditorGUI.EndChangeCheck ()) {
			UnityEditor.Undo.RecordObject (material, "ScreenSpaceNoiseDissolve");
			EditorUtility.SetDirty (material);

			// Color
			material.SetColor ("_Color", color);

			// Texture
			material.SetTexture ("_MainTex", mainTex);
			material.SetTexture ("_MetallicTex", metalTex);

			// Metal
			if (metalTex == null) {
				material.SetFloat ("_UseMetallicTex", 0);
				material.SetFloat ("_Metallic", metallic);
			} else {
				material.SetFloat ("_UseMetallicTex", 1);
			}
			material.SetFloat ("_Glossiness", smoothness);

			// Noise
			material.SetFloat ("_NoiseSize", noiseSize);

			// Dissolve
			material.SetFloat ("_DissolvePercentage", dissolvePercentage);

			// Rim
			material.SetFloat ("_RimSize", rimSize * 0.1f);
			material.SetColor ("_RimColor", rimColor);
			material.SetFloat ("_RimIntensity", rimIntensity);
			material.SetFloat ("_EnhancedRimColor", enhancedRimColor);
		}
	}
}
