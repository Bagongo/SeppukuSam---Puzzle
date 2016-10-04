using UnityEngine;
using System.Collections;

public class OldNpc : RegNpc {

	public bool movesEveryOtherTurn = true;
	public bool hasMovedPrevTurn = false;


	public override void ElaborateMove()
	{
		if(currentPos[1] < 2)
			movesEveryOtherTurn = false;
									
		if(movesEveryOtherTurn && hasMovedPrevTurn)
		{
			hasMovedPrevTurn = false;
			PosRecorder(currentPos);
			SortNextMove();
		}
		else
		{
			hasMovedPrevTurn = true;
			base.ElaborateMove();
		}

	}

	public void RewindPrevTurnStates(int howManyTurns)
	{
		for(int i=1; i <= howManyTurns * nmbrOfMoves; i++)
			hasMovedPrevTurn = !hasMovedPrevTurn;
	}
}