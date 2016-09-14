using UnityEngine;
using System.Collections;

public class OldNpc : RegNpc {

	public bool movesEveryOtherTurn = true;

	private bool hasMovedPrevTurn = false;


	public override void ElaborateMove()
	{
		if(currentPos[1] < 2);
			movesEveryOtherTurn = false;
									
		if(movesEveryOtherTurn && hasMovedPrevTurn)
		{
			hasMovedPrevTurn = false;
			SortNextMove();
		}
		else
		{
			hasMovedPrevTurn = true;
			base.ElaborateMove();
		}

	}
}