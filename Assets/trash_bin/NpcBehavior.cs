using UnityEngine;
using System.Collections;

public class NpcBehavior : EntityBehavior, IMovable {

	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
							
		if(IsInBoundsX(requestedPos))
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null)
				newPos = requestedPos;
			else
				newPos = TryNeighbor(requestedPos, Random.value <= .5 ? 1 : -1); 
		}
		else
			newPos = currentPos;	

		return newPos;
	}

}
