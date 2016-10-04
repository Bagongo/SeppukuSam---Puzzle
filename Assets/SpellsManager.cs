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


	//deal with animation stopping....
	public void RewindMoves(int rewindsNmr)
	{
		int idx = boardMan.snapShots.Count - rewindsNmr - 1;
		idx = Mathf.Clamp(idx, 0, idx);
		Dictionary<GameObject, int[]> snapToLoad = boardMan.snapShots[idx];
		Dictionary<GameObject, int[]> snapClone = new Dictionary<GameObject, int[]>();

		foreach(KeyValuePair<GameObject, int[]> pair in snapToLoad)
			snapClone.Add(pair.Key, pair.Value);

		boardMan.ClearBoardFromEntities();

		foreach(KeyValuePair<GameObject, int[]> pair in snapClone)
		{
			if (pair.Key != null)
			{
				pair.Key.GetComponent<EntityBehavior>().hasKnife = false; // temporary solution...?
				boardMan.InstantiateSingleEntity(pair.Key, pair.Value);
			} 
		}

		boardMan.RemoveSnapshots(rewindsNmr, true);											
		turnMan.turnNmr -= rewindsNmr;
	}


}
