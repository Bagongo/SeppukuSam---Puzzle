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

	//add Player blocking system....
	//deal with animation stopping....
	public void RewindMoves(int rewindsNmr)
	{
		Debug.Log(boardMan.snapShots.Count);

		if(boardMan.snapShots.Count > rewindsNmr)
		{
			int idx = boardMan.snapShots.Count-1 - rewindsNmr;
			idx = Mathf.Clamp(idx, 0, boardMan.snapShots.Count - 1);
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
			turnMan.turnNmr = Mathf.Clamp(turnMan.turnNmr, 0, turnMan.turnThreshold);
		}
		else
			Debug.Log("Not enough snaps to rewind.....");

		Debug.Log(boardMan.snapShots.Count);

					
	}


}
