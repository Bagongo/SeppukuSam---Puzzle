using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable]
public class ColorPalette : ScriptableObject {

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Color Palette")]
	public static void CreateColorPalette()
	{
		if(Selection.activeObject is Texture2D)
		{
			var selectedTexture = Selection.activeObject as Texture2D;
			var selectionPath = AssetDatabase.GetAssetPath(selectedTexture);

			selectionPath = selectionPath.Replace(".png", "-color-palette.asset");

			var newPalette = CustomAssetUtil.CreateAsset<ColorPalette>(selectionPath);

			newPalette.source = selectedTexture;
			newPalette.ResetPalette();

			Debug.Log("Creating a palette.... " + selectionPath);
		}
		else
		{
			Debug.Log("Can't create a palette!!!");
		}

	}
	#endif

	public Texture2D source;
	public List<Color> palette = new List<Color>();
	public List<Color> newPalette = new List<Color>();
	public Texture2D cachedTexture;

	private List<Color> BuildPalette(Texture2D texture)
	{
		List<Color> palette = new List<Color>();

		var colors = texture.GetPixels();

		foreach(var color in colors)
		{
			if(!palette.Contains(color))
			{
				if(color.a == 1)
				{
					palette.Add(color);
				}
			}
		}

		return palette;
	}

	public void ResetPalette()
	{
		palette = BuildPalette(source);
		newPalette = new List<Color>(palette); 		
	}

	public Color GetColor(Color color)
	{
		for(int i=0; i<palette.Count; i++)
		{
			Color tempCol = palette[i];

			if(Mathf.Approximately(color.r, tempCol.r) &&
				Mathf.Approximately(color.g, tempCol.g) &&
				Mathf.Approximately(color.b, tempCol.b) &&
				Mathf.Approximately(color.a, tempCol.a))
			{
				return newPalette[i];
			}
		}

		return color;
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(ColorPalette))]
public class CustomPaletteEditor: Editor{

	public ColorPalette colorPalette;

	void OnEnable()
	{
		colorPalette = target as ColorPalette;
	}

	override public void OnInspectorGUI()
	{
		GUILayout.Label("Source texture");
		colorPalette.source = EditorGUILayout.ObjectField(colorPalette.source, typeof(Texture2D), false) as Texture2D;

		GUILayout.BeginHorizontal();
		GUILayout.Label("Current color");
		GUILayout.Label("New color");
		GUILayout.EndHorizontal();

		for(int i=0; i<colorPalette.palette.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.ColorField(colorPalette.palette[i]);

			if(GUILayout.Button("<"))
				colorPalette.newPalette[i] = EditorGUILayout.ColorField(colorPalette.palette[i]);

			colorPalette.newPalette[i] = EditorGUILayout.ColorField(colorPalette.newPalette[i]);

			EditorGUILayout.EndHorizontal();	
		}

		if(GUILayout.Button("Revert Palette"))
		{
			colorPalette.ResetPalette();
		}

		EditorUtility.SetDirty(colorPalette);

	}
}
#endif
