using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellsManager : MonoBehaviour {

	private TurnMan turnMan;
	private BoardMan boardMan;

	void Awake()
	{
		turnMan = (TurnMan)FindObjectOfType(typeof(TurnMan));
		boardMan = (BoardMan)FindObjectOfType(typeof(BoardMan));

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void RewindMoves(int rewindsNmr)
	{
		for(int y = boardMan.gridH - 1; y>=2; y--)
		{
			for(int x=0; x<boardMan.gridW; x++)
			{
				if(boardMan.entities[x,y] != null)
				{
					EntityBehavior entB = boardMan.entities[x,y].GetComponent<EntityBehavior>();

					if(entB.formerPositions.Count < rewindsNmr * entB.nmbrOfMoves)
					{
						entB.EliminateEntity();
						Destroy(entB.gameObject);
					}
					else
					{
						int idx = entB.formerPositions.Count - rewindsNmr * entB.nmbrOfMoves;
						int[] backToPos = entB.formerPositions[idx];

						for(int i= idx; i < entB.formerPositions.Count; i++)
							entB.formerPositions.RemoveAt(idx);

						if(entB.GetComponent<OldNpc>())
							entB.GetComponent<OldNpc>().RewindPrevTurnStates(rewindsNmr);

						boardMan.UpdateGrid(entB.currentPos, backToPos);

						entB.currentPos[0] = backToPos[0];
						entB.currentPos[1] = backToPos[1];
						entB.transform.position = boardMan.grid[backToPos[0], backToPos[1]].transform.position;
					}
				}
			}
		}

		turnMan.turnNmr -= rewindsNmr;
	}


}
