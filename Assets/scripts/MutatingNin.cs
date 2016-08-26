using UnityEngine;
using System.Collections;

public class MutatingNin : MonoBehaviour {

	public GameObject[] disguises;
	public GameObject realID;
	public RegEnemy regEnemy;

	private GameObject disguise;
	private SpriteRenderer originalSpriteRend;
	private SpriteRenderer altSpriteRend;
	private RegNpc regNpc;
	private Color originalCol;
	private float speed;

	void Awake()
	{
		GetComponent<PaletteSwap>().enabled = false;
		originalSpriteRend = GetComponent<SpriteRenderer>(); 

		disguise = disguises[Random.Range(0, disguises.Length -1)];

		originalCol = originalSpriteRend.color;
		altSpriteRend = disguise.GetComponent<SpriteRenderer>();
		originalSpriteRend.sprite = altSpriteRend.sprite;

		regNpc = gameObject.AddComponent(typeof(RegNpc)) as RegNpc;
		regNpc.isMutant = true;

		speed = regNpc.speed;
	}

	public bool EvaluateMutation()
	{
		if(regNpc.currentPos[1] < regNpc.gridH - 3 && regNpc.currentPos[1] > 4 && Random.value < 0.4f)
			return true;
		else
			return false;

	}

	public void PrepareMutation()
	{
		originalSpriteRend.color = new Color(originalCol.r, originalCol.g, originalCol.b, 0);
		originalSpriteRend.sprite = realID.GetComponent<SpriteRenderer>().sprite;

		GetComponent<PaletteSwap>().enabled = true;

		StartCoroutine(Appear());
	}

	IEnumerator Appear()
	{
		Color color = originalSpriteRend.color;
		float tempOpacity = color.a;

		while(tempOpacity < 1)
		{
			tempOpacity += speed * Time.deltaTime;
			color = new Color(color.r, color.g, color.b, tempOpacity);
			originalSpriteRend.color = color;

			yield return null;
		}

		CompleteMutation();
	}

	void CompleteMutation()
	{
		regEnemy = gameObject.AddComponent(typeof(RegEnemy)) as RegEnemy;
		regNpc.CompensateMoves(1);
		regNpc.FinalizeMovement();
		Destroy(regNpc);
	}


}
