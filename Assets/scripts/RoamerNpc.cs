using UnityEngine;
using System.Collections;

public class RoamerNpc : EntityBehavior, IMovable {

	public bool switchedDir = false;

	void Start(){

		moveDirection[0] = Random.value < 0.5f ? -1 : 1;
	}
	
	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
							
		if(IsInBoundsX(requestedPos) && boardMan.entities[requestedPos[0], requestedPos[1]] == null)
		{
			newPos = requestedPos;
			switchedDir = false;
		}
		else
		{
			if(!switchedDir)
			{		
				moveDirection[0] = -moveDirection[0];
				switchedDir = true;
				return EvaluateMovement();
			}
			else if(boardMan.entities[currentPos[0], currentPos[1] - 1] == null)
				newPos = new int[]{currentPos[0], currentPos[1] - 1};
			else 
				newPos = currentPos;
		}

		switchedDir = false;
		return newPos;
	}	
}
