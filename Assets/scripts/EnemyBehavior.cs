using UnityEngine;
using System.Collections;

public class EnemyBehavior : EntityBehavior, IMovable, IAttacker{

	public int[] EvaluateMovement()
	{
		int[] requestedPos = new int[]{currentPos[0]+moveDirection[0], currentPos[1]+moveDirection[1]};
		int[] newPos;
									
		if(IsInBoundsX(requestedPos))
		{
			if(boardMan.entities[requestedPos[0], requestedPos[1]] != null) //modify to make attack only if stuck....
			{
				Attack(requestedPos);
				newPos = requestedPos;
			}
			else if(boardMan.entities[requestedPos[0], requestedPos[1]] == null)
				newPos = requestedPos;
			else
				newPos = TryNeighbor(requestedPos, Random.value <= .5 ? 1 : -1); 
		}
		else
		{
			 newPos = currentPos;
		} 

		return newPos;
	}


	public void Attack(int[] targetPos)
	{
		EntityBehavior entB = boardMan.entities[targetPos[0], targetPos[1]].GetComponent<EntityBehavior>();
		entB.EliminateEntity();
	}
		
}
