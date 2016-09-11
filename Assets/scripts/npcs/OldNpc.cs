using UnityEngine;
using System.Collections;

public class OldNpc : RegNpc {

	public bool movesEveryOtherTurn = true;

	private bool hasMovedPrevTurn = false;

	public override void ElaborateMove()
	{
		if(movesEveryOtherTurn && hasMovedPrevTurn)
		{
			StartCoroutine(WaitToResolve(0.25f));
			hasMovedPrevTurn = false;
		}
		else
		{
			hasMovedPrevTurn = true;
			base.ElaborateMove();	
		}														
	}

	IEnumerator WaitToResolve(float time)
	{
		 yield return new WaitForSeconds(time);
		 SortNextMove();
	}


}
