﻿using UnityEngine;
using System.Collections;

public class RegNpc : EntityBehavior, IMovable {

	public virtual int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
									
		if(IsInBoundsX(requestedPos))
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] == null || boardMan.entities[requestedPos[0], requestedPos[1]] == this.gameObject)
				newPos = requestedPos;
			else
				newPos = TryNeighbor(requestedPos); 
		}
		else
			newPos = currentPos;	

		return newPos;
	}
}
