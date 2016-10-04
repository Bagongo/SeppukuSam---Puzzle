using UnityEngine;
using System.Collections;

public class MutatingNin : EntityBehavior {

	public GameObject[] disguises;

	private GameObject disguise;
	private GameObject npc;
	private RegNpc npcB;
	private GameObject realID;
	private RegEnemy eneB;
	private SpriteRenderer sprtRen;
	private float opacityIncrementFact;

	void Start () {

		CreateDisguise();
		InitializeRealID();
		InitializaDisguise();
	}

	void CreateDisguise()
	{
		disguise = disguises[Random.Range(0, disguises.Length)];
		npc = boardMan.InstantiateSingleEntity(disguise, currentPos);
		transform.parent = npc.transform;
	}

	void InitializeRealID()
	{
		realID = transform.GetChild(0).gameObject;
		eneB = realID.GetComponent<RegEnemy>();
		sprtRen = eneB.GetComponent<SpriteRenderer>();
		opacityIncrementFact = eneB.speed;
	}

	void InitializaDisguise()
	{
		npcB = npc.GetComponent<RegNpc>();
		npcB.isMutant = true;
		npcB.canPickUp = false;
		npcB.canDrop = false;
		npcB.honorValue = eneB.honorValue;
		npcB.scoreValue = eneB.scoreValue;
	}

	public bool EvaluateMutation()
 	{
		if(npcB.currentPos[1] < npcB.gridH - 3 && npcB.currentPos[1] > 4) //&& Random.value < 0.4f)
 			return true;
 		else
 			return false; 
 	}

	public void PrepareMutation()
	{
		turnMan.totalMovesToClear += eneB.nmbrOfMoves;
		npcB.EraseRemainingMoves();
		//npcB.CompensateMoves(1);
		//eneB.movesCompleted = eneB.nmbrOfMoves - 1;
		eneB.nextPos = npcB.currentPos;

		transform.parent = null;
		realID.transform.parent = null;
		transform.parent = realID.transform;
	
		npcB.EliminateEntity();
		npcB.DestroyEntity();

		boardMan.entities[eneB.nextPos[0], eneB.nextPos[1]] = eneB.gameObject;
			
		StartCoroutine("Appear");
	}

	IEnumerator Appear()
	{
		Color newColor = sprtRen.color;
		float tempOpacity = newColor.a;

		while(tempOpacity < 1f)
		{
			tempOpacity += opacityIncrementFact * Time.deltaTime;
			newColor = new Color(newColor.r, newColor.g, newColor.b, tempOpacity);
			sprtRen.color = newColor;

			yield return null;
		}

		eneB.FinalizeMovement();
	}

}
