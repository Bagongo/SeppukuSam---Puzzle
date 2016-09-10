using UnityEngine;
using System.Collections;

public class OldNpc : RegNpc {

	public bool movesEveryOtherTurn = true; 

	private bool hasMovedPrevTurn = false;

	void Start () {

	}

	public override void ElaborateMove()
	{
		if(movesEveryOtherTurn && hasMovedPrevTurn)
		{
			hasMovedPrevTurn = false;
			EraseRemainingMoves();
		}
		else
		{
			hasMovedPrevTurn = true;
			base.ElaborateMove();	
		}												
	}


}
