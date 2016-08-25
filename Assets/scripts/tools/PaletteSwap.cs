using UnityEngine;
using System.Collections;

public class PaletteSwap : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public ColorPalette[] colorPalettes;

	private Texture2D texture;
	private MaterialPropertyBlock block;

	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer>();

		if(colorPalettes.Length > 0)
			SwapColors(colorPalettes[Random.Range(0, colorPalettes.Length)]);
	}

	void SwapColors(ColorPalette palette)
	{
		if(palette.cachedTexture == null)
		{
			texture = spriteRenderer.sprite.texture;

			int w = texture.width;
			int h = texture.height;

			 var cloneTexture = new Texture2D(w, h);
			cloneTexture.wrapMode = TextureWrapMode.Clamp;
			cloneTexture.filterMode = FilterMode.Point;

			var colors = texture.GetPixels();

			for(int i=0; i<colors.Length; i++)
			{
				colors[i] = palette.GetColor(colors[i]);
			}

			cloneTexture.SetPixels(colors);
			cloneTexture.Apply();
			palette.cachedTexture = cloneTexture;
		}

		block = new MaterialPropertyBlock();
		block.SetTexture("_MainTex", palette.cachedTexture);
	}

	void LateUpdate()
	{
		spriteRenderer.SetPropertyBlock(block);
	}
}
